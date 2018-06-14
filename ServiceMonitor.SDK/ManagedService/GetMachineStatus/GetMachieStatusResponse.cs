using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetMachieStatusResponse: BaseResponse
    {
        public MachineStatus Result { get; set; }
    }
}
