using Chainway.ServiceMonitor.SDK;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.BLL
{
    public class WindowsServiceBusiness
    {
        ServerBusiness serverbll = new ServerBusiness();

        /// <summary>
        /// 获得所有服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public List<WindowsService> GetAllServices(string serverID)
        {
            List<WindowsService> list = new List<WindowsService>();
            GetAllServicesRequest request = new GetAllServicesRequest();
            var server = serverbll.GetServer(serverID);
            Client client = new Client();
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Services;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool StartService(string serverID, string serviceName)
        {
            var server = serverbll.GetServer(serverID);
            Client client = new Client();
            StartWSRequest request = new StartWSRequest();
            string url = client.GetRealUrl(request, server.Url);
            request.name = serviceName;
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Success;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public bool StopService(string serverID, string serviceName)
        {
            var server = serverbll.GetServer(serverID);
            Client client = new Client();
            StopWSRequest request = new StopWSRequest();
            string url = client.GetRealUrl(request, server.Url);
            request.name = serviceName;
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Success;
        }

        /// <summary>
        /// 获得服务信息
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public WindowsService GetServiceInfo(string serverID, string serviceName)
        {
            var server = serverbll.GetServer(serverID);
            Client client = new Client();
            GetWSInfoRequest request = new GetWSInfoRequest();
            string url = client.GetRealUrl(request, server.Url);
            request.name = serviceName;
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Service;
        }
    }
}
