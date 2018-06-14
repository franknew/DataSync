
using Chainway.ServiceMonitor.BLL;
using Chainway.ServiceMonitor.SDK;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Chainway.ServiceMonitor.Controllers
{
    [ExceptionHandler]
    public class ServiceController : ApiController
    {
        private ServerBusiness _bll = new ServerBusiness();
        private SimpleLogger _logger = new SimpleLogger();


        [AcceptVerbs("GET", "POST")]
        public List<Server> GetServers()
        {
            return _bll.GetServers();
        }

        [AcceptVerbs("GET", "POST")]
        public List<Command> GetServices(string serverID)
        {
            return _bll.GetServices(serverID);
        }

        [AcceptVerbs("GET", "POST")]
        public List<Command> GetRunningServices(string serverID)
        {
            return _bll.GetRunningServices(serverID);
        }

        [AcceptVerbs("GET", "POST")]
        public bool StartService(string serverID, string serviceID)
        {
            return _bll.StartService(serverID, serviceID);
        }

        [AcceptVerbs("GET", "POST")]
        public bool StopService(string serverID, string serviceID)
        {
            return _bll.StopService(serverID, serviceID);
        }

        [AcceptVerbs("GET", "POST")]
        public Command GetServiceInfo(string serverID, string serviceID)
        {
            return _bll.GetServiceInfo(serverID, serviceID);
        }

        [AcceptVerbs("GET", "POST")]
        public bool CheckValid(string serverID, string serviceID)
        {
            return _bll.CheckValid(serverID, serviceID);
        }

        [AcceptVerbs("GET", "POST")]
        public ServicePanel GetServicePanel(string serverID)
        {
            ServicePanelBusiness bll = new ServicePanelBusiness();
            return bll.GetService(serverID);
        }

        [AcceptVerbs("GET", "POST")]
        public bool Ping(string serverID)
        {
            return _bll.Ping(serverID);
        }

        [HttpGet]
        public MachineStatus GetMachineStatus(string serverID)
        {
            return _bll.GetMachineStatus(serverID);
        }
    }
}
