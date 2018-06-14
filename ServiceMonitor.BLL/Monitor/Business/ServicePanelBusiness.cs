using Chainway.ServiceMonitor.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.BLL
{
    public class ServicePanelBusiness
    {
        ServerBusiness serverbll = new ServerBusiness();
        WindowsServiceBusiness wsbll = new WindowsServiceBusiness();

        public ServicePanel GetService(string serverID)
        {
            ServicePanel service = new ServicePanel();
            service.ManagedServiceList = serverbll.GetServices(serverID);
            service.WindowsServiceList = wsbll.GetAllServices(serverID);
            return service;
        }
    }
}
