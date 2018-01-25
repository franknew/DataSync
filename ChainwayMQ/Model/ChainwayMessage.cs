
using org.apache.rocketmq.common.message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class ChainwayMessage : Message
    {
        public ChainwayMessage(string topic)
            : base()
        {
            this.setTopic(topic);
        }
        public ChainwayMessage(string topic, byte[] body)
            : base(topic, body)
        { }


        public ChainwayMessage(string topic, byte[] body, bool waitStoreMsgOK)
            : base(topic, null, null, -1, body, waitStoreMsgOK)
        { }

        /// <summary>
        /// 传输的内容
        /// </summary>
        public string Body
        {
            get { return Encoding.UTF8.GetString(this.getBody()); }
            set { this.setBody(Encoding.UTF8.GetBytes(value)); }
        }
    }
}