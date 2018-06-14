using AustinHarris.JsonRpc;
using Chainway.ServiceMonitor.SDK;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.BLL
{
    [JsonRpcClass]
    public class MonitorController
    {
        [JsonRpcMethod]
        [ExceptionFilter]
        public MachineStatus GetMachineStatus()
        {
            MachineStatus status = new MachineStatus();
            Performance performance = new Performance();
            status.CpuRating = performance.GetCurrentCpuUsage();
            status.FreeMem = performance.GetAvailableRamSize();
            status.TotalMem = performance.GetTotalMem();
            status.MachineName = performance.GetMachineName();
            return status;
        }
    }
}
