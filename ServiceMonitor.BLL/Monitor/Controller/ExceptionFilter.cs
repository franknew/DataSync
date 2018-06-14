using MicroService.Library;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor
{
    public class ExceptionFilter : BaseFilterAttribute
    {
        public override HttpFilterResult OnException(HttpServerContext context, Exception ex)
        {
            SimpleLogger logger = new SimpleLogger();
            logger.WriteException(ex);
            return base.OnException(context, ex);
        }
    }
}
