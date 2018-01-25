using java.util;
using org.apache.rocketmq.client.consumer;
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chainway.Library.MQ
{
    /// <summary>
    /// 消息推送型消费者
    /// </summary>
    public class ChainwayPushConsumer : DefaultMQPushConsumer
    {
        public ChainwayPushConsumer()
            : base()
        {
            this.registerMessageListener(new ChainwayMessageListener(OnConsumeMessage));
        }

        public ChainwayPushConsumer(string group)
            : base(group)
        {
            this.registerMessageListener(new ChainwayMessageListener(OnConsumeMessage));
        }

        public bool ConsumeBlock { get; set; }

        public bool Connected { get; private set; }

        public void Start()
        {
            this.start();
            Connected = true;
        }

        public void Shutdown()
        {
            this.shutdown();
            Connected = false;
        }

        public void Close()
        {
            this.Close();
            Connected = false;
        }

        /// <summary>
        /// 消费事件,需要注册该事件才能进行消息的推送
        /// </summary>
        public event ConsumeEventHandler ConsumeMessage;

        public ConsumeConcurrentlyStatus OnConsumeMessage(List l, ConsumeConcurrentlyContext ccc)
        {
            while (ConsumeBlock) Thread.Sleep(100);
            List<Message> list = l.toArray().Cast<Message>().ToList();
            if (ConsumeMessage != null) return ConsumeMessage.Invoke(this, new ConsumeEventArgs { Messages = list, Context = ccc });
            else throw new Exception("没有注册ConsumeMessage事件");
        }
    }
}