using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class WindowsService
    {
        public string ServiceName { get; set; }

        public string DisplayName { get; set; }

        public ServiceControllerStatus Status { get; set; }

        public ServiceType Type { get; set; }

        public string MachineName { get; set; }
    }
}
