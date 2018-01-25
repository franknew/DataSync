using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Chainway.SyncData.Pipe
{
    public class PipeWorkderManager
    {
        private Dictionary<string, PipeWorker> _workerDic = new Dictionary<string, PipeWorker>();
        private List<PipeConfig> _config = null;

        public Dictionary<string, PipeWorker> WorkerDic { get => _workerDic; set => _workerDic = value; }
        public List<PipeConfig> Config { get => _config; set => _config = value; }

        public event PipeReceiveEventHandler OnPipeReceiveMessage;

        public PipeWorkderManager()
        {
            LoadConfig();
        }

        public void InitWorker()
        {
            _config.ForEach(t =>
            {
                CreateWorker(t);
            });
        }

        public PipeWorker CreateWorker(PipeConfig config)
        {
            PipeWorker worker;
            worker = new PipeWorker(config.Host, config.Port, config.BindingPort);
            worker.Start();
            _workerDic[config.Host] = worker;
            return worker;
        }

        public PipeWorker GetWorker(PipeConfig config)
        {
            PipeWorker worker;
            if (_workerDic.ContainsKey(config.Host)) worker = _workerDic[config.Host];
            else worker = CreateWorker(config);
            return worker;
        }

        private void LoadConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Config/pipe.json";
            if (!File.Exists(path)) throw new Exception("配置文件pipe.json不存在");
            string json = File.ReadAllText(path);
            Config = JsonHelper.Deserialize<List<PipeConfig>>(json);
        }
    }
}
