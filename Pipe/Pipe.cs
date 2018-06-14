using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Chainway.SyncData.Pipe
{
    public delegate void ReceiveMessageHandler(byte[] data);

    public class Pipe: IDisposable
    {
        Socket _socket;

        public Pipe(Socket socket = null)
        {
            if (socket != null) Socket = socket;
            else Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
        }

        public event ReceiveMessageHandler OnReceiveMessage;
        public event EventHandler OnReceiveTimeOut;

        public bool Connected { get => Socket.Connected; }
        public Socket Socket { get => _socket; set => _socket = value; }

        public void Bind(IPEndPoint iPEnd)
        {
            Socket.Bind(iPEnd);
        }

        public void Connect(IPEndPoint iPEnd)
        {
            Socket.Connect(iPEnd);
        }

        public void Disconnect()
        {
            Socket.Disconnect(true);
        }

        public bool Send(byte[] data)
        {
            lock (this)
            {
                var list = data.ToList();
                list.Add(0);
                return Socket.Send(list.ToArray()) > 1;
            }
        }

        public byte[] Receive(byte[] buffer = null)
        {
            lock (this)
            {
                List<byte> list = new List<byte>();
                while (true)
                {
                    byte[] temp = new byte[1];
                    try
                    {
                        Socket.Receive(temp);
                        if (temp[0] == 0 && list.Count > 0)
                        {
                            byte[] data = new byte[list.Count];
                            list.CopyTo(data);
                            if (buffer != null) list.CopyTo(buffer);
                            Thread th = new Thread(OnReceive);
                            th.Start(data);
                            list = new List<byte>();
                            return list.ToArray();
                        }
                        else if (temp[0] == 0 && list.Count == 0) continue;
                        else if (temp[0] != 0) list.Add(temp[0]);
                    }
                    catch (SocketException ex)
                    {
                        if (ex.SocketErrorCode == SocketError.TimedOut)
                        {
                            OnReceiveTimeOut?.Invoke(this, new EventArgs());
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        return list.ToArray();
                    }
                }
            }
        }

        public void Receive(object buffer)
        {
            byte[] data = buffer as byte[];
            Receive(data);
        }

        public void ReceiveSync()
        {
            byte[] buffer = new byte[1024];
            Thread th = new Thread(Receive);
            th.Start(buffer);
        }

        public void Listen(int count)
        {
            Socket.Listen(count);
        }

        public Pipe Accept()
        {
            Pipe client = null;
            Socket socket = null;
            try
            {
                socket = Socket.Accept();
            }
            catch (Exception ex)
            {
                socket = Socket.Accept();
            }
            client = new Pipe(socket);
            return client;
        }

        public void Shutdown(SocketShutdown shutdown)
        {
            Socket.Shutdown(shutdown);
        }

        public void Close()
        {
            Socket.Close(30 * 1000);
        }

        public void Dispose()
        {
            Socket?.Dispose();
        }

        private void OnReceive(object sender)
        {
            var data = sender as byte[];
            if (OnReceiveMessage != null) OnReceiveMessage.Invoke(data);
        }
    }
}
