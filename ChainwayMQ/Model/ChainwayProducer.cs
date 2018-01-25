
using org.apache.rocketmq.client.producer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class ChainwayProducer : DefaultMQProducer
    {
        public ChainwayProducer()
            : base()
        { }

        public ChainwayProducer(string group)
            : base(group)
        { }

        public event SendExceptionHandler OnException;
        public event SendSuccessHandler OnSuccess;

        public void Send(ChainwayMessage message)
        {
            ChainwaySendCallback callback = new ChainwaySendCallback(this)
            {
                ExceptionHandler = OnException,
                SendSuccessHandler = OnSuccess,
            };
            this.send(message, callback);
        }
    }
}