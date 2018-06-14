using Chainway.ServiceMonitor.SDK;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chainway.ServiceMonitor.BLL
{
    public class MonitorBusiness
    {
        private List<Command> _msConfig;
        private List<WindowsService> _wsConfig;
        private SimpleLogger _logger = new SimpleLogger();

        public bool Stop { get; set; }
        public List<Command> ManagedServiceConfig { get => _msConfig; set => _msConfig = value; }
        public List<WindowsService> WindowsServiceConfig { get => _wsConfig; set => _wsConfig = value; }

        public MonitorBusiness()
        {
            LoadManagedServiceConfig();
            LoadWindowsServiceConfig();
            FileSystemWatcher watcher = new FileSystemWatcher(new FileInfo(GetConfigFilePath()).Directory.FullName);
            watcher.Filter = "*.json";
            watcher.Changed += Watcher_Changed;
        }

        /// <summary>
        /// 获得配置中的进程
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Process GetProcess(Command config)
        {
            string fileName = config.Name.Remove(0, config.Name.LastIndexOf("\\") + 1);
            var processes = Process.GetProcessesByName(fileName.Remove(fileName.Length - 4, 4));
            if (processes == null || processes.Length == 0) return null;
            foreach (var p in processes)
            {
                var commandLine = GetCommandLine(p);
                if (string.IsNullOrEmpty(config.Argruments)) return p;
                else if (!string.IsNullOrEmpty(config.Description) && commandLine.Equals(config.Description?.Trim())) return p;
                else if (string.IsNullOrEmpty(config.Description) && commandLine.Equals(config.Argruments?.Trim())) return p;
            }
            return null;
        }

        /// <summary>
        /// 启动进程
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Process StartProcess(Command config)
        {
            var current = new Process();
            current.StartInfo = new ProcessStartInfo
            {
                FileName = string.IsNullOrEmpty(config.Executor) ? config.Name : config.Executor,
                UseShellExecute = false,
                CreateNoWindow = false,

                Arguments = config.Argruments,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            if (!string.IsNullOrEmpty(config.Path)) current.StartInfo.WorkingDirectory = config.Path;
            _logger.Write(current.StartInfo.WorkingDirectory);
            _logger.Write(current.StartInfo.FileName);
            current.Start();
            return current;
        }

        public bool StartService(Command config)
        {
            var services = ServiceController.GetServices();
            var service = services.FirstOrDefault(t => t.ServiceName.ToLower().Equals(config.Name.ToLower()));
            if (service == null) return false;
            if (service.Status != ServiceControllerStatus.Running) service.Start();
            return true;
        }

        /// <summary>
        /// 启动对服务的监视
        /// </summary>
        public void StartWatcher()
        {
            _logger.Write(string.Format("{0}个服务正在等待监测", ManagedServiceConfig.Count));
            Stop = false;
            while (!Stop)
            {
                try
                {
                    foreach (var c in ManagedServiceConfig)
                    {
                        var current = GetProcess(c);
                        _logger.Write(string.Format("{1}:{0}", current == null, c.ID));
                        if (current != null && c.Stop) current.Close();
                        else if ((current != null && !c.Stop) || (current == null && c.Stop)) continue;
                        if (c.Delay > 0) Thread.Sleep(c.Delay * 1000);
                        if (c.IsWindowsService) StartService(c);
                        else StartProcess(c);
                        _logger.Write("已启动服务" + c.Path + "\\" + c.Name + " " + c.Argruments);
                    }
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                }
                Thread.Sleep(60 * 1000);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void StopWatcher()
        {
            Stop = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            LoadManagedServiceConfig();
            LoadWindowsServiceConfig();
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        private void LoadManagedServiceConfig()
        {
            string json = File.ReadAllText(GetManagedServiceConfigFileName());
            ManagedServiceConfig = JsonHelper.Deserialize<List<Command>>(json);
        }

        private void LoadWindowsServiceConfig()
        {
            string json = File.ReadAllText(GetWindowsServiceConfigFileName());
            WindowsServiceConfig = JsonHelper.Deserialize<List<WindowsService>>(json);
        }

        /// <summary>
        /// 获得配置文件地址
        /// </summary>
        /// <returns></returns>
        public string GetConfigFilePath()
        {
            string path = ConfigurationManager.AppSettings["ConfigPath"];
            if (string.IsNullOrEmpty(path)) path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\Config\";
            return path.TrimEnd('\\') + "\\";
        }

        /// <summary>
        /// 获得配置文件名
        /// </summary>
        /// <returns></returns>
        public string GetManagedServiceConfigFileName()
        {
            return GetConfigFilePath() + "config.json";
        }

        public string GetWindowsServiceConfigFileName()
        {
            return GetConfigFilePath() + "ws.json";
        }

        /// <summary>
        /// 写入配置
        /// </summary>
        public void WriteManagedServiceConfig()
        {
            string json = JsonHelper.Serialize(ManagedServiceConfig);
            string configFile = GetManagedServiceConfigFileName();
            File.WriteAllText(configFile, json);
        }

        /// <summary>
        /// 获得命令行
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static string GetCommandLine(Process process)
        {
            var commandLine = new StringBuilder();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    commandLine.Append(@object["CommandLine"]);
                }
            }
            string command = commandLine.ToString();
            if (command.StartsWith("\""))
            {
                command = command.TrimStart('\"');
                command = command.Remove(0, command.IndexOf("\"") + 1).Trim();
            }
            return command;
        }
    }
}
