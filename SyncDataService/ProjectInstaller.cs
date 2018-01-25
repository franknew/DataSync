using Chainway.Library.MQ;
using SOAFramework.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;

namespace SyncDataService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private SimpleLogger _logger = new SimpleLogger(@"D:\");
        public ProjectInstaller()
        {
            InitializeComponent();
            FileInfo file = new FileInfo(this.GetType().Assembly.Location);
            MQFactory.LoadConfig(file.Directory.FullName);
            //System.IO.Directory.SetCurrentDirectory(System.AppDomain.Cur‌​rentDomain.BaseDirec‌​tory);
            if (!string.IsNullOrEmpty(MQFactory.Consumerconfig[0].ServiceName)) this.serviceInstaller1.ServiceName = this.serviceInstaller1.DisplayName = MQFactory.Consumerconfig[0].ServiceName;
        }

    }
}
