
using org.apache.rocketmq.client.producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class ChainwaySendCallback : SendCallback
    {
        public SendExceptionHandler ExceptionHandler;
        public SendSuccessHandler SendSuccessHandler;
        MQProducer _producer = null;

        public ChainwaySendCallback(MQProducer producer)
        {
            _producer = producer;
        }

        public void onException(Exception t)
        {
            ExceptionHandler?.Invoke(_producer, new SendExceptionEvent { Ex = t });
        }

        public void onSuccess(SendResult sr)
        {
            SendSuccessHandler?.Invoke(_producer, new SendSuccessArgs { Result = sr });
        }
    }
}
