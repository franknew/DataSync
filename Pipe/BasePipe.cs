
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
    public class PipeRecieveEventArgs : EventArgs
    {
        public string Data { get; set; }

        public Pipe Client { get; set; }
    }

    public class ConnectedEventArgs : EventArgs
    {
        public PipeHandler Client { get; set; }
    }

    public delegate void PipeReceiveEventHandler(object sender, PipeRecieveEventArgs args);
    public delegate void ConnectedEventHandler(object sender, ConnectedEventArgs args);

    public abstract class BasePipe
    {
        public BasePipe(string ip, int port, int bindingPort)
        {
            _ip = ip;
            _port = port;
            _bindingPort = bindingPort;
        }

        protected string _ip;
        protected int _port;
        protected int _bindingPort;

        protected Pipe _pipe = null;

        public Pipe Pipe { get => _pipe; set => _pipe = value; }
        

        public string Host
        {
            get { return _ip; }
        }

        public bool Connected { get; set; }

        public void Start()
        {
            Init();
            Work();
        }

        protected void Init()
        {
            _pipe = new Pipe();
            if (_bindingPort > 0) _pipe.Bind(new IPEndPoint(IPAddress.Any, _bindingPort));
        }

        public void Close()
        {
            _pipe.Close();
        }

        public abstract void Work();

        public void ReciveMessage(object socket)
        {
            try
            {
                Pipe client = socket as Pipe;
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    client.Receive(buffer);
                    string json = Encoding.UTF8.GetString(buffer);
                    PipeData data = new PipeData
                    {
                        Data = json,
                        Client = client,
                    };
                    Thread thread = new Thread(HandleMessage);
                    thread.Start(data);
                }
            }
            catch (SocketException ex)
            { }
        }

        protected abstract void HandleMessage(object data);

        protected abstract void HandleConnected(object e);
    }

    public class PipeData
    {
        public Pipe Client { get; set; }

        public string Data { get; set; }
    }
}
