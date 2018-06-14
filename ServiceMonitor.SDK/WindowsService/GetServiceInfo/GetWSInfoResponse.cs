using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.SDK
{
    public class GetWSInfoResponse : CommonResponse
    {
        public WindowsService Service { get; set; }
    }
}
