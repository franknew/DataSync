using AustinHarris.JsonRpc;
using Chainway.ServiceMonitor.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Chainway.ServiceMonitor.BLL
{
    [JsonRpcClass]
    public class WindowsServiceController
    {
        [JsonRpcMethod]
        [ExceptionFilter]
        public bool Start(string name)
        {
            var service = GetService(name);
            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running);
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool Stop(string name)
        {
            var service = GetService(name);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped);
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool Pause(string name)
        {
            var service = GetService(name);
            service.Pause();

            service.WaitForStatus(ServiceControllerStatus.Paused);
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool Continue(string name)
        {
            var service = GetService(name);
            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Running);
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public WindowsService GetServiceInfo(string name)
        {
            var service = GetService(name);
            return ServiceToEntity(service);
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public List<WindowsService> GetAllServices()
        {
            MonitorBusiness bll = new MonitorBusiness();
            if (bll.WindowsServiceConfig == null) return new List<WindowsService>();
            return GetServices(t =>
            {
                return bll.WindowsServiceConfig.Any(p =>
                (!string.IsNullOrEmpty(p.DisplayName) && p.DisplayName.Equals(t.DisplayName))
                || (!string.IsNullOrEmpty(p.ServiceName) && p.ServiceName.Equals(t.ServiceName))
                );
            });
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public List<WindowsService> GetServices(Func<ServiceController, bool> fitler)
        {
            List<WindowsService> list = new List<WindowsService>();
            var services = ServiceController.GetServices();
            foreach (var service in services)
            {
                if (fitler == null) list.Add(ServiceToEntity(service));
                else if (fitler != null && fitler(service)) list.Add(ServiceToEntity(service));
            }
            return list;
        }

        private ServiceController GetService(string name)
        {
            var services = ServiceController.GetServices();
            var service = services.FirstOrDefault(t => t.ServiceName.ToLower().Equals(name.ToLower()) || t.DisplayName.ToLower().Equals(name.ToLower()));
            if (service == null) throw new Exception(string.Format("服务名称:{0} 在本机不存在", name));
            return service;
        }

        private WindowsService ServiceToEntity(ServiceController service)
        {
            if (service == null) return null;
            WindowsService entity = new WindowsService
            {
                ServiceName = service.ServiceName,
                DisplayName = service.DisplayName,
                Status = service.Status,
                Type = service.ServiceType,
                MachineName = service.MachineName,
            };
            return entity;
        }
    }
}
