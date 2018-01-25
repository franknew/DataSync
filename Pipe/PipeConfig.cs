using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Pipe
{
    public class PipeConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public int BindingPort { get; set; }

        public string FullHost
        {
            get { return Host + ":" + Port; }
        }
    }
}
