using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetWSInfoRequest : IRequest<GetWSInfoResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.WindowsServiceController.GetServiceInfo";
        }

        public string name { get; set; }
    }
}
