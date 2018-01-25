using Chainway.Library.MQ;
using Chainway.SyncData.Pipe;
using org.apache.rocketmq.client.consumer.listener;
using SOAFramework.Library;
using SOAFramework.Library.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chainway.SyncData.BLL
{
    public class PipeProxyV2 : PipeProxy
    {
        private Dictionary<string, PipeHandler> _workerDic = new Dictionary<string, PipeHandler>();
        public override void Start()
        {
            _logger.Write("启动中。。");
            _manager.InitWorker();
            _logger.Write("已启动");

            List<Task> list = new List<Task>();
            int count = 0;
            List<Task> inner = new List<Task>();
            MQFactory.Consumerconfig.ForEach(t =>
            {
                var innerTask = new Task(() =>
                {
                    StartWatcher(t, ref count);
                });
                inner.Add(innerTask);

                Task producerTask = new Task(() =>
                {
                    StartProducer(t);
                });
                producerTask.Start();
                innerTask.Start();
            });
        }

        private void StartWatcher(ChainwayMQConfig t, ref int count)
        {
            while (true)
            {
                if (Stop) break;
                _logger.Write("正在准备连接服务器：" + t.ProxyIP);
                var config = _manager.Config.Find(p => p.Host.Equals(t.ProxyIP));
                var worker = _manager.GetWorker(config);
                try
                {
                    var hanlder = new PipeHandler(worker.Pipe);
                    _workerDic[t.ProxyIP] = hanlder;
                    _logger.Write("已连接，准备完成");
                    Ready(hanlder);
                    _logger.Write("开始接收消息队列数据");
                    count = Consume(worker, Stop);
                    _logger.Write("处理消息队列结束，准备发送结束信息");
                    SendFinish(hanlder);
                    _logger.Write("服务器：" + t.ProxyIP + "发送了" + count.ToString() + "条消费消息");
                    _logger.Write("接收消息队列数据结束");

                    var produceconfig = MQFactory.Producerconfig.Find(p => p.ProxyIP.Equals(t.ProxyIP));
                    count = ReceiveMsg(hanlder, produceconfig, Stop);
                    _logger.Write("从服务器：" + produceconfig.ProxyIP + "接受了" + count.ToString() + "生产消息，并推送到消息队列");
                }
                catch (SocketException ex)//发生socket异常，重连
                {
                    _logger.WriteException(ex);
                    Thread.Sleep(1000 * 10);
                    worker.Close();
                    _manager.InitWorker();
                }
                catch (TimeoutException ex)//发生超时异常，重连
                {
                    _logger.WriteException(ex);
                    Thread.Sleep(1000 * 10);
                    worker.Close();
                    _manager.InitWorker();
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                    _manager.InitWorker();
                }

                Thread.Sleep(1000 * 5);
            }
        }

        private void StartProducer(ChainwayMQConfig config)
        {
            var producer = MQFactory.CreatePushComsumer(config)[0];
            producer.ConsumeMessage += Producer_ConsumeMessage;
            producer.Start();
        }

        public void Close()
        {
            this.Stop = true;
            foreach (var key in _workerDic.Keys)
            {
                _workerDic[key].Close();
            }
        }

        private int Consume(PipeWorker workder, bool stop = false)
        {
            int count = 0;
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT TOP 1000 * FROM MQCache WHERE [HasError]=0 AND Enabled=1 ORDER BY [CreateTime] ASC");
            IDBHelper helper = DBFactory.CreateDBHelper();
            DataTable table = helper.GetTableWithSQL(sql.ToString());
            while (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (stop) return count;
                    string id = row["ID"].ToString();
                    string group = row["Group"].ToString();
                    Contract contract = new Contract
                    {
                        Command = MessageTypeEnum.Work,
                        Data = row["Body"].ToString(),
                        Keys = row["Key"].ToString(),
                        Tags = row["Tag"].ToString(),
                    };

                    var response = SendMessage(group, contract, workder);
                    if (response.Command == MessageTypeEnum.Error)
                    {
                        StringBuilder update = new StringBuilder();
                        update.Append("UPDATE MQCache SET [HasError]=1,[ErrorMessage]=@Message WHERE ID=@ID");
                        List<Parameter> parameter = new List<Parameter>
                        {
                            new Parameter{ Name = "@Message", Value = response.Message },
                            new Parameter{ Name = "@ID", Value = id }
                        };
                        helper.ExecNoneQueryWithSQL(update.ToString());
                    }
                    else
                    {
                        StringBuilder delete = new StringBuilder();
                        delete.Append("UPDATE MQCache SET [Enabled]=0 WHERE ID=" + id);
                        helper.ExecNoneQueryWithSQL(delete.ToString());
                        count++;
                    }
                }
                table = helper.GetTableWithSQL(sql.ToString());
            }
            _logger.Write("发送了" + count + "条消费数据到主机");
            return count;
        }

        private ConsumeConcurrentlyStatus Producer_ConsumeMessage(object obj, ConsumeEventArgs args)
        {
            IDBHelper helper = DBFactory.CreateDBHelper();
            //存储队列
            var generator = IDGeneratorFactory.Create(GeneratorType.SnowFlak);
            foreach (var m in args.Messages)
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    string ID = generator.Generate();
                    string body = Encoding.UTF8.GetString(m.getBody());
                    string tableName = m.getKeys();
                    string tag = m.getTags();

                    sql.Append("INSERT INTO MQCache([ID],[Key],[Tag],[Body],[CreateTime]) VALUES(@ID, @Key, @Tag, @Body, GETDATE())");
                    List<Parameter> parameters = new List<Parameter>
                    {
                    new Parameter("@ID", ID),
                    new Parameter("@Key", tableName),
                    new Parameter("@Tag", tag),
                    new Parameter("@Body", body),
                    };
                    lock (this)
                    {
                        helper.ExecNoneQueryWithSQL(sql.ToString(), parameters.ToArray());
                    }
                    _logger.Write("插入数据库成功,json:" + body);
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                    throw ex;
                }
            }


            return ConsumeConcurrentlyStatus.CONSUME_SUCCESS;
        }
    }
}
