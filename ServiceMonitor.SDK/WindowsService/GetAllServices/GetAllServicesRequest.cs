using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetAllServicesRequest : IRequest<GetAllServicesResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.WindowsServiceController.GetAllServices";
        }
    }
}
