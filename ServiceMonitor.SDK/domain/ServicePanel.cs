using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class ServicePanel
    {
        public List<Command> ManagedServiceList { get; set; }
        public List<WindowsService> WindowsServiceList { get; set; }
    }
}
