using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class MachineStatus
    {
        public float CpuRating { get; set; }

        public long TotalMem { get; set; }

        public long FreeMem { get; set; }

        public string MachineName { get; set; }
        
    }
}
