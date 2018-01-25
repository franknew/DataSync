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
using System.Threading.Tasks;

namespace TableWatcherService
{
    public partial class TableWatcher : ServiceBase
    {
        private WatcherManager _manager = new WatcherManager();
        private SimpleLogger _logger = new SimpleLogger();
        public TableWatcher()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logger.Write("数据监视服务正在启动。。。");
            var t = Task.Factory.StartNew(() =>
            {
                _logger.Write("数据监视进入启动线程。");
                _manager.StartTableWatcher(MQFactory.Producerconfig[0]);
            });
            _logger.Write("数据监视服务已启动。。。");
            Console.ReadLine();
        }

        protected override void OnStop()
        {
            _manager.StopTableWatcher();
        }
    }
}
