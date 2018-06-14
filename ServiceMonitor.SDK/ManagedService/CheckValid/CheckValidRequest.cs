using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class CheckValidRequest : IRequest<CommonResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.ManagedServiceController.CheckValid";
        }

        public string id { get; set; }
    }
}
