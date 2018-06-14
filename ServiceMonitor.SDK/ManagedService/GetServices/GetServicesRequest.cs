using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetServicesRequest : IRequest<GetServicesResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.ManagedServiceController.GetAllServices";
        }
    }
}
