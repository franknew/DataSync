using Chainway.Library.MQ;
using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class MQProxyHandler : IProxyHandler
    {
        private ChainwayProducer _producer;

        public MQProxyHandler(ChainwayProducer producer)
        {
            _producer = producer;
        }

        public Contract Recevieve()
        {
            throw new NotImplementedException();
        }

        public void Send(Contract data)
        {
            ChainwayMessage message = new ChainwayMessage(data.Topic) { Body = data.Data };
            message.setTags(data.Tags);
            message.setKeys(data.Keys);
            var result = _producer.send(message);
        }
    }
}
