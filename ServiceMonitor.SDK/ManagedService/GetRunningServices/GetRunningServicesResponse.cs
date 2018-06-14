using Chainway.ServiceMonitor.SDK;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetRunningServicesResponse : BaseResponse
    {
        public List<Command> List { get; set; }
    }
}
