
using org.apache.rocketmq.client;
using org.apache.rocketmq.client.consumer;
using org.apache.rocketmq.client.consumer.store;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    /// <summary>
    /// 消息拉取型消费者
    /// </summary>
    public class ChainwayPullConsumer : DefaultMQPullConsumer
    {
        public ChainwayPullConsumer()
            : base()
        { }

        public ChainwayPullConsumer(string group)
            : base()
        { }

        public bool Connected { get; private set; }

        public void Start()
        {
            this.start();
            this.Connected = true;
        }

        public void Shutdown()
        {
            this.shutdown();
            this.Connected = false;
        }

        public void UpdateOffset(MessageQueue queue, long offset)
        {
            var instance = this.getDefaultMQPullConsumerImpl().getmQClientFactory();
            ClientConfig clientConfig = this.cloneClientConfig();
            RemoteBrokerOffsetStore store = new RemoteBrokerOffsetStore(instance, this.getConsumerGroup());
            store.updateConsumeOffsetToBroker(queue, offset, true);
            this.setOffsetStore(store);
            this.updateConsumeOffset(queue, offset);
        }
    }
}