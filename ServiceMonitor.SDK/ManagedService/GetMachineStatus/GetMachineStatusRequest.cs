using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetMachineStatusRequest : IRequest<GetMachieStatusResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.MonitorController.GetMachineStatus";
        }
    }
}
