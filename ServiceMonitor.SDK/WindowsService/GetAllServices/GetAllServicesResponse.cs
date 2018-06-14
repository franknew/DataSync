using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetAllServicesResponse: CommonResponse
    {
        [SDKResult]
        public List<WindowsService> Services { get; set; }
    }
}
