using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class StartServiceRequest : IRequest<CommonResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.ManagedServiceController.StartService";
        }

        public string id { get; set; }
    }
}
