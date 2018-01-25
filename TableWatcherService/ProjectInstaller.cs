using Chainway.Library.MQ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;

namespace TableWatcherService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            FileInfo file = new FileInfo(this.GetType().Assembly.Location);
            MQFactory.LoadConfig(file.Directory.FullName);
            //System.IO.Directory.SetCurrentDirectory(System.AppDomain.Cur‌​rentDomain.BaseDirec‌​tory);
            if (!string.IsNullOrEmpty(MQFactory.Producerconfig[0].ServiceName)) this.serviceInstaller1.ServiceName = this.serviceInstaller1.DisplayName = MQFactory.Producerconfig[0].ServiceName;
        }
    }
}
