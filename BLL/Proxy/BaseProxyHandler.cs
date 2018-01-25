using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class BaseProxyHandler : IProxyHandler
    {
        private IProxyHandler _proxy;
        public BaseProxyHandler(IProxyHandler proxy)
        {
            _proxy = proxy;
        }

        public virtual void Send(Contract data)
        {
            _proxy.Send(data);
        }

        public virtual Contract Recevieve()
        {
            return _proxy.Recevieve();
        }
    }
}
