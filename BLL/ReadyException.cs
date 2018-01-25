using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class ReadyException : Exception
    {
        public ReadyException(string msg): base (msg)
        { }
    }
}
