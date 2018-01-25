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
    public class PipeHost : BasePipe
    {
        private Pipe _client = null;
        private SimpleLogger _logger = new SimpleLogger();

        public PipeHost(int bindingPort)
            : base(null, -1, bindingPort)
        { }

        public event PipeReceiveEventHandler OnPipeReceive;

        public event ConnectedEventHandler OnConnected;

        public override void Work()
        {
            _pipe.Listen(100);
            Thread myThread = new Thread(new ThreadStart(Listen));
            myThread.Start();
            while (!this.Connected)
            {
                Thread.Sleep(100);
            }
        }

        protected override void HandleConnected(object e)
        {
            var args = e as ConnectedEventArgs;
            if (OnConnected != null) OnConnected.Invoke(this, args);
        }

        protected void Listen()
        {
            while (true)
            {
                var client = Pipe.Accept();
                this.Connected = true;
                ConnectedEventArgs args = new ConnectedEventArgs { Client = new PipeHandler(client) };
                Thread th = new Thread(HandleConnected);
                th.Start(args);
            }
        }

        protected override void HandleMessage(object data)
        {
            var pipeData = data as PipeData;

            if (OnPipeReceive != null) OnPipeReceive.Invoke(this, new PipeRecieveEventArgs { Data = pipeData.Data, Client = pipeData.Client });
        }
    }
}
