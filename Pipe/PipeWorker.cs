using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chainway.SyncData.Pipe
{
    
    public class PipeWorker : BasePipe
    {
        private SimpleLogger _logger = new SimpleLogger();

        public PipeWorker(string ip, int port, int bindingPort)
            :base (ip, port, bindingPort)
        {  }

        public event PipeReceiveEventHandler OnPipeReceive;

        public override void Work()
        {
            while (!_pipe.Connected)
            {
                try
                {
                    if (!string.IsNullOrEmpty(_ip) && !_pipe.Connected) _pipe.Connect(new IPEndPoint(IPAddress.Parse(_ip), _port));
                }
                catch (Exception ex)
                {
                    _logger.WriteException(ex);
                    Thread.Sleep(5000);
                }
            }
        }

        protected override void HandleConnected(object e)
        {
        }

        protected override void HandleMessage(object data)
        {
            PipeData pipeData = data as PipeData;
            if (OnPipeReceive != null) OnPipeReceive.Invoke(this, new PipeRecieveEventArgs { Data = pipeData.Data, Client = _pipe });
        }
    }
}
