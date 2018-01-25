
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.Library.MQ
{
    public class ChainwayMQConfig
    {
        public string Group { get; set; }
        public List<string> Topic { get; set; }
        public string Address { get; set; }
        public string ServiceName { get; set; }
        public int Top { get; set; }
        public bool EnableLog { get; set; }
        public bool EnableSql { get; set; }
        public int Port { get; set; }
        public string ProxyIP { get; set; }
    }
}