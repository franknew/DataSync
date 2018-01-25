using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class TimeoutException : Exception
    {
        public TimeoutException(string message) :
            base(message)
        { }
    }
}
