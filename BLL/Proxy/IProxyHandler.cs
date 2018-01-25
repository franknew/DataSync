using Chainway.Library.SimpleMapper;
using Chainway.SyncData.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public interface IProxyHandler
    {
        void Send(Contract data);

        Contract Recevieve();
    }
}
