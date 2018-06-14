using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chainway.ServiceMonitor.Controllers
{
    public class ExceptionHandler : FilterAttribute, IExceptionFilter
    {
        private SimpleLogger _logger = new SimpleLogger();

        public void OnException(ExceptionContext filterContext)
        {
            _logger.WriteException(filterContext.Exception);
        }
    }
}