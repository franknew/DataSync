using Chainway.ServiceMonitor.SDK;
using SOAFramework.Library;
using SOAFramework.Service.SDK.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chainway.ServiceMonitor.BLL
{
    public class ServerBusiness
    {
        private List<Server> _config;

        public ServerBusiness()
        {
            _config = LoadConfig();
        }

        public List<Server> LoadConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\Config\\server.json";
            string json = File.ReadAllText(path);
            return JsonHelper.Deserialize<List<Server>>(json);
        }
        
        /// <summary>
        /// 获得所有监控服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public List<Command> GetServices(string serverID)
        {
            List<Command> commands = new List<Command>();
            var server = GetServer(serverID);
            Client client = new Client();
            GetServicesRequest request = new GetServicesRequest();
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            commands = response?.List;
            return commands;
        }

        /// <summary>
        /// 获得正在运行的监控服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public List<Command> GetRunningServices(string serverID)
        {
            List<Command> commands = new List<Command>();
            var server = GetServer(serverID);
            Client client = new Client();
            GetRunningServiceRequest request = new GetRunningServiceRequest();
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            commands = response?.List;
            return commands;
        }

        /// <summary>
        /// 启动一个服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool StartService(string serverID, string serviceID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            StartServiceRequest request = new StartServiceRequest();
            request.id = serviceID;
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);

            return response.Success;
        }

        /// <summary>
        /// 停止一个服务
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool StopService(string serverID, string serviceID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            StopServiceRequest request = new StopServiceRequest();
            request.id = serviceID;
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);

            return response.Success;
        }

        /// <summary>
        /// 获得所有服务器配置
        /// </summary>
        /// <returns></returns>
        public List<Server> GetServers()
        {
            return _config;
        }

        public bool Ping(string serverID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            PingRequest request = new PingRequest();
            string url = client.GetRealUrl(request, server.Url);

            var response = SDKFactory.Client.Execute(request, url: url);
            return response.Success;
        }

        public Command GetServiceInfo(string serverID, string serviceID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            GetServiceInfoRequest request = new GetServiceInfoRequest();
            string url = client.GetRealUrl(request, server.Url);
            request.id = serviceID;
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Service;
        }

        public bool CheckValid(string serverID, string serviceID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            CheckValidRequest request = new CheckValidRequest();
            request.id = serviceID;
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Success;
        }
        
        public Server GetServer(string serverID)
        {
            var server = _config.Find(t => t.ID.Equals(serverID));
            if (server == null) throw new Exception("serverID:" + serverID + "不存在");
            return server;
        }

        public MachineStatus GetMachineStatus(string serverID)
        {
            var server = GetServer(serverID);
            Client client = new Client();
            GetMachineStatusRequest request = new GetMachineStatusRequest();
            string url = client.GetRealUrl(request, server.Url);
            var response = SDKFactory.Client.Execute(request, url: url);
            if (response.IsError) throw new Exception(response?.ErrorMessage);
            return response.Result;
        }
    }
}
