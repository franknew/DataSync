
using org.apache.rocketmq.client.producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public delegate void SendSuccessHandler(object sender, SendSuccessArgs args);

    public class SendSuccessArgs : EventArgs
    {
        public SendResult Result { get; set; }
    }
}
