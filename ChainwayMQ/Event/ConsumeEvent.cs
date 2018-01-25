
using org.apache.rocketmq.client.consumer.listener;
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class ConsumeEventArgs : EventArgs
    {
        public List<Message> Messages { get; set; }

        public ConsumeConcurrentlyContext Context { get; set; }
    }

    /// <summary>
    /// 消息推送事件定义
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public delegate ConsumeConcurrentlyStatus ConsumeEventHandler(object obj, ConsumeEventArgs args);


}