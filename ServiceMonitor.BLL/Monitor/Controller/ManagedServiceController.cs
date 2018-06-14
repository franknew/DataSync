using AustinHarris.JsonRpc;
using Chainway.ServiceMonitor;
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
    [JsonRpcClass]
    public class ManagedServiceController
    {
        private MonitorBusiness _bll = new MonitorBusiness();
        private SimpleLogger _logger = new SimpleLogger();

        [JsonRpcMethod]
        [ExceptionFilter]
        public List<Command> GetAllServices()
        {
            return _bll.ManagedServiceConfig;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public List<Command> GetRuningServices()
        {
            List<Command> services = new List<Command>();
            var config = _bll.ManagedServiceConfig;
            foreach (var c in _bll.ManagedServiceConfig)
            {
                var p = _bll.GetProcess(c);
                if (p != null) services.Add(c);
            }
            return services;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool StartService(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("没有id");
            var c = _bll.ManagedServiceConfig.Find(t => t.ID.ToLower().Equals(id.ToLower()));
            var p = _bll.GetProcess(c);
            c.Stop = false;
            _bll.WriteManagedServiceConfig();
            if (p != null) return true;
            p = _bll.StartProcess(c);
            return p != null;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool StopService(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("没有id");
            //设置配置
            var c = _bll.ManagedServiceConfig.Find(t => t.ID.ToLower().Equals(id.ToLower()));
            var p = _bll.GetProcess(c);
            if (p == null) return true;
            c.Stop = true;
            _bll.WriteManagedServiceConfig();
            p.Kill();
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool CheckValid(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("没有id");
            //设置配置
            var c = _bll.ManagedServiceConfig.Find(t => t.ID.ToLower().Equals(id.ToLower()));
            var p = _bll.GetProcess(c);
            if (p == null) return false;
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool ClearLogHistory()
        {
            string configPath = _bll.GetConfigFilePath();
            DirectoryInfo dirConfig = new DirectoryInfo(configPath);
            string logPath = dirConfig.Parent.FullName + @"Logs\";
            DirectoryInfo dirLog = new DirectoryInfo(logPath);
            var files = dirLog.GetFiles("*.log");
            foreach (var file in files)
            {
                var fileName = DateTime.Now.ToString("yyyyMMdd");
                if (!file.Name.StartsWith(fileName)) file.Delete();
            }
            return true;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public Command GetServiceInfo(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new Exception("没有id");
            //设置配置
            var c = _bll.ManagedServiceConfig.Find(t => t.ID.ToLower().Equals(id.ToLower()));
            var p = _bll.GetProcess(c);
            c.Stop = p == null;
            return c;
        }

        [JsonRpcMethod]
        [ExceptionFilter]
        public bool Ping()
        {
            return true;
        }
    }
}
