using Chainway.ServiceMonitor.Controllers;
using System.Web;
using System.Web.Mvc;

namespace Chainway.ServiceMonitor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandler());
        }
    }
}
