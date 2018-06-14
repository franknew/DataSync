
using Chainway.ServiceMonitor.BLL;
using MicroService.Library;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor
{
    public partial class Monitor : ServiceBase
    {
        //MonitorBusiness bll = new MonitorBusiness();
        private SimpleLogger _logger = new SimpleLogger();
        //private MasterServer _server = new MasterServer(); 

        public Monitor()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //string displayShell = ConfigurationManager.AppSettings["DisplayShell"];
                //_server.DisplayShell = displayShell == "1";
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        _logger.Write("监视服务正在启动");
                        MonitorBusiness bll = new MonitorBusiness();
                        bll.StartWatcher();
                        _logger.Write("监视服务已启动");
                    }
                    catch (Exception ex)
                    {
                        _logger.WriteException(ex);
                    }
                });

                //Task.Factory.StartNew(() =>
                //{
                //    _logger.Write("http服务正在启动");
                //    _server.Start();
                //    _logger.Write("http服务已启动");
                //});
            }
            catch (Exception ex)
            {
                _logger.WriteException(ex);
            }
            Console.ReadLine();
        }

        protected override void OnStop()
        {
            //bll.StopWatcher();
            //_server.CloseAllNode();
            _logger.Write("服务已停止");
        }
    }
}
