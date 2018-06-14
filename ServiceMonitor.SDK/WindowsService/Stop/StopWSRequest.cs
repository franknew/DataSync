using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class StopWSRequest : IRequest<CommonResponse>
    {
        public string GetApi()
        {
            return "Chainway.ServiceMonitor.BLL.WindowsServiceController.Stop";
        }

        public string name { get; set; }
    }
}
