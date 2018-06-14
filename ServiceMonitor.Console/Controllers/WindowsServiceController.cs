using Chainway.ServiceMonitor.BLL;
using Chainway.ServiceMonitor.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chainway.ServiceMonitor.Controllers
{
    [ExceptionHandler]
    public class WindowsServiceController : ApiController
    {
        private WindowsServiceBusiness bll = new WindowsServiceBusiness();

        [AcceptVerbs("GET", "POST")]
        public List<WindowsService> GetAllServices(string serverID)
        {
            return bll.GetAllServices(serverID);
        }


        [AcceptVerbs("GET", "POST")]
        public WindowsService GetServiceInfo(string serverID, string serviceName)
        {
            return bll.GetServiceInfo(serverID, serviceName);
        }

        [AcceptVerbs("GET", "POST")]
        public bool StartService(string serverID, string serviceName)
        {
            return bll.StartService(serverID, serviceName);
        }

        [AcceptVerbs("GET", "POST")]
        public bool StopService(string serverID, string serviceName)
        {
            return bll.StopService(serverID, serviceName);
        }
    }
}
