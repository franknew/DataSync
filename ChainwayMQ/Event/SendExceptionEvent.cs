using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public delegate void SendExceptionHandler(object sender, SendExceptionEvent args);

    public class SendExceptionEvent : EventArgs
    {
        public Exception Ex { get; set; }
    }
}
