using Chainway.Library.MQ;
using Chainway.SyncData.Pipe;
using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chainway.SyncData.BLL
{
    public class Common
    {
        public static FileSystemWatcher CreateFileWatcher(string path)
        {
            FileSystemWatcher fileWatcher = new FileSystemWatcher();
            fileWatcher.Filter = "*.json";
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileWatcher.Path = path;
            fileWatcher.Changed += fileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
            return fileWatcher;
        }

        private static void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            MQFactory.LoadConfig();
        }

        public static List<PipeConfig> LoadPipeConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Config/pipe.json";
            if (!File.Exists(path)) throw new Exception("配置文件pipe.json不存在");
            string json = File.ReadAllText(path);
            var Config = JsonHelper.Deserialize<List<PipeConfig>>(json);
            return Config;
        }
    }
}
