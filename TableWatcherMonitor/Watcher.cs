using Chainway.Library.MQ;
using Chainway.SyncData.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableWatcherMonitor
{
    public partial class Watcher : Form
    {
        private WatcherManager _manager = new WatcherManager();
        public Watcher()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                _manager.StartTableWatcher(MQFactory.Producerconfig[0]);
            });
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _manager.StopTableWatcher();
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }
    }
}
