using Chainway.Library.MQ;
using Chainway.SyncData.Pipe;
using org.apache.rocketmq.client;
using org.apache.rocketmq.client.consumer;
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.client.consumer.store;
using org.apache.rocketmq.client.impl;
using org.apache.rocketmq.client.impl.factory;
using org.apache.rocketmq.common;
using org.apache.rocketmq.common.message;
using org.apache.rocketmq.common.protocol.header;
using org.apache.rocketmq.remoting;
using org.apache.rocketmq.remoting.netty;
using org.apache.rocketmq.remoting.protocol;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chainway.SyncData.BLL
{
    public class PipeProxy
    {
        protected PipeWorkderManager _manager = new PipeWorkderManager();
        protected Dictionary<string, ChainwayProducer> _producerDic = new Dictionary<string, ChainwayProducer>();
        protected Dictionary<string, ChainwayPullConsumer> _consumerDic = new Dictionary<string, ChainwayPullConsumer>();
        protected SimpleLogger _logger = new SimpleLogger();

        public bool Stop { get; set; }

        #region public action
        public virtual void Start()
        {
            _logger.Write("启动中。。");
            _manager.InitWorker();

            List<Task> list = new List<Task>();
            int count = 0;
            List<Task> inner = new List<Task>();
            MQFactory.Consumerconfig.ForEach(t =>
            {
                var innerTask = new Task(() =>
                {
                    while (true)
                    {
                        _logger.Write("正在准备连接服务器：" + t.ProxyIP);
                        var config = _manager.Config.Find(p => p.Host.Equals(t.ProxyIP));
                        var worker = _manager.GetWorker(config);
                        var hanlder = new PipeHandler(worker.Pipe);
                        try
                        {
                            _logger.Write("已连接，准备完成");
                            Ready(hanlder);
                            _logger.Write("开始接收消息队列数据");
                            count = StartConsumer(t, worker);
                            SendFinish(hanlder);
                            _logger.Write("服务器：" + t.ProxyIP + "发送了" + count.ToString() + "条消费消息");

                            var produceconfig = MQFactory.Producerconfig.Find(p => p.ProxyIP.Equals(t.ProxyIP));
                            count = ReceiveMsg(hanlder, produceconfig);
                            _logger.Write("从服务器：" + produceconfig.ProxyIP + "接受了" + count.ToString() + "生产消息，并推送到消息队列");
                        }
                        catch (SocketException ex)//发生socket异常，重连
                        {
                            _logger.WriteException(ex);
                            worker.Close();
                            _manager.CreateWorker(config);
                            Thread.Sleep(1000 * 10);
                        }
                        catch (TimeoutException ex)//发生超时异常，重连
                        {
                            _logger.WriteException(ex);
                            worker.Close();
                            _manager.CreateWorker(config);
                            Thread.Sleep(1000 * 10);
                        }
                        catch (Exception ex)
                        {
                            _logger.WriteException(ex);
                        }

                        Thread.Sleep(1000 * 5);
                    }
                });
                inner.Add(innerTask);
                innerTask.Start();
            });
        }

        public ChainwayProducer GetProducer(string groupName, string nameAddress)
        {
            ChainwayProducer producer = null;
            if (_producerDic.ContainsKey(groupName)) producer = _producerDic[groupName];
            else
            {
                producer = MQFactory.CreateProducer(groupName, nameAddress, -1);
                _producerDic[groupName] = producer;
                producer.start();
            }
            return producer;
        }
        #endregion

        #region private action


        protected int StartConsumer(ChainwayMQConfig config, PipeWorker worker)
        {
            ChainwayPullConsumer consumer;
            int count = 0;
            config.Topic.ForEach(p =>
            {
                if (!_consumerDic.ContainsKey(config.Group))
                {
                    consumer = MQFactory.CreatePullComsumer(config.Group, config.Address, -1);
                    _consumerDic[config.Group] = consumer;
                }
                else consumer = _consumerDic[config.Group];
                if (!consumer.Connected) consumer.Start();
                var messages = consumer.fetchSubscribeMessageQueues(p);
                var messagesTemp = messages.toArray();
                long offset = 0;
                foreach (MessageQueue queue in messagesTemp)
                {
                    offset = consumer.fetchConsumeOffset(queue, true);
                    if (offset < 0) offset = 0;
                    var result = consumer.pull(queue, "*", offset, 10000);
                    if (result.getPullStatus() != PullStatus.FOUND) continue;
                    var list = result.getMsgFoundList()?.toArray();
                    if (list == null) continue;
                    foreach (Message item in list)
                    {
                        if (Consumer_ConsumeMessage(consumer.getConsumerGroup(), item, worker) == ConsumeConcurrentlyStatus.CONSUME_SUCCESS)
                        {
                            count++;
                            consumer.UpdateOffset(queue, offset + 1);
                            offset = result.getNextBeginOffset();
                        }
                        else break;
                    }
                }
            });
            return count;
        }

        protected ConsumeConcurrentlyStatus Consumer_ConsumeMessage(string group, Message m, PipeWorker worker)
        {
            string body = Encoding.UTF8.GetString(m.getBody());
            string tableName = m.getKeys();
            string tag = m.getTags();
            Contract contract = new Contract
            {
                Data = body,
                Keys = tableName,
                Tags = tag,
                Command = MessageTypeEnum.Work,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
            };
            var response = SendMessage(group, contract, worker);
            if (response.Command == MessageTypeEnum.Error) return ConsumeConcurrentlyStatus.RECONSUME_LATER;
            _logger.Write("成功接收一条数据，数据：" + body);
            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }

        protected Contract SendMessage(string group, Contract contract, PipeWorker worker)
        {
            string json = JsonHelper.Serialize(contract);
            var config = MQFactory.Consumerconfig.Find(t => t.Group.Equals(group));
            PipeHandler handler = new PipeHandler(worker.Pipe);
            handler.Send(json);
            var responseString = handler.Receive(30);
            if (string.IsNullOrEmpty(responseString)) throw new TimeoutException("接收消费回执超时");
            var response = JsonHelper.Deserialize<Contract>(responseString);
            return response;
        }

        protected int ReceiveMsg(PipeHandler worker, ChainwayMQConfig config, bool stop = false)
        {
            int count = 0;
            while (true)
            {
                if (stop) break;
                string json = worker.Receive(120);
                if (string.IsNullOrEmpty(json)) throw new TimeoutException("接收主机信息超时");
                Contract data = JsonHelper.Deserialize<Contract>(json); 
                switch (data.Command)
                {
                    case MessageTypeEnum.Work:
                        Contract result = new Contract();
                        try
                        {
                            ChainwayProducer producer = GetProducer(config.Group, config.Address);
                            foreach (var t in config.Topic)
                            {
                                ChainwayMessage msg = new ChainwayMessage(t);
                                msg.setKeys(data.Keys);
                                msg.setTags(data.Tags);
                                msg.Body = data.Data;
                                producer.send(msg);
                                count++;
                                _logger.Debug(string.Format("表{0}成功推送1条数据到消息队列,json:{1}", data.Keys, data.Data));
                            }
                            result.Command = MessageTypeEnum.OK;
                        }
                        catch (Exception ex)
                        {
                            result.Command = MessageTypeEnum.Error;
                            result.Message = ex.Message;
                            _logger.WriteException(ex);
                        }
                        worker.Send(result);
                        break;
                    case MessageTypeEnum.End:
                        return count;
                }
            }
            return count;
        }

        protected void SendFinish(PipeHandler worker)
        {
            Contract end = new Contract { Command = MessageTypeEnum.End };
            var json = JsonHelper.Serialize(end);
            worker.Send(json);
        }

        public void Ready(PipeHandler worker)
        {
            Contract contract = new Contract { Command = MessageTypeEnum.Ready };
            worker.Send(contract);
        }
        #endregion
    }
}
