

using java.util;
using org.apache.rocketmq.client.consumer.listener;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public delegate ConsumeConcurrentlyStatus OnConsumeDelegate(List l, ConsumeConcurrentlyContext ccc);

    /// <summary>
    /// 消息监听器
    /// </summary>
    public class ChainwayMessageListener : MessageListenerConcurrently
    {
        private OnConsumeDelegate _onconsumedelegate = null;
        

        public ChainwayMessageListener(OnConsumeDelegate onconsume)
        {
            _onconsumedelegate = onconsume;
        }

        public ConsumeConcurrentlyStatus consumeMessage(List l, ConsumeConcurrentlyContext ccc)
        {
            if (_onconsumedelegate != null) return _onconsumedelegate.Invoke(l, ccc);
            
            else throw new Exception("没有初始化代理OnConsumeDelegate");
        }
    }


}