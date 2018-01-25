using Chainway.Library.MQ;
using Chainway.SyncData.BLL;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncDataService
{
    public partial class SyncData : ServiceBase
    {
        private SimpleLogger _logger = new SimpleLogger();
        private ConsumerManager _manager;
        public SyncData()
        {
            InitializeComponent();
            _manager = new ConsumerManager();
        }

        protected override void OnStart(string[] args)
        {
            _logger.Write("同步服务正在启动。。。");
            _manager.StartConsumer(MQFactory.Consumerconfig[0]);
            _logger.Write("同步服务已启动。。。");
            Console.ReadLine();
        }

        protected override void OnStop()
        {
            _logger.Write("同步服务正在停止。。。");
            _manager.StopConsumer();
            _logger.Write("同步服务已停止。。。");
        }
    }
}
