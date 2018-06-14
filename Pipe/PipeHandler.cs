using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Pipe
{
    public class PipeHandler: IDisposable
    {
        Pipe _pipe = null;

        public Pipe Pipe { get => _pipe; set => _pipe = value; }

        public PipeHandler(Pipe pipe)
        {
            Pipe = pipe;
        }

        public void Send(string data)
        {
            var list = Encoding.UTF8.GetBytes(data).ToList();
            list.Add(0);
            Pipe.Send(list.ToArray());
        }

        public void Send(object obj)
        {
            string json = JsonHelper.Serialize(obj);
            Send(json);
        }

        public string Receive(int timeout = -1)
        {
            string result = null;
            try
            {
                if (timeout > 0) Pipe.Socket.ReceiveTimeout = timeout * 1000;
                byte[] data = new byte[1024 * 100];
                Pipe.Receive(data);
                result = Encoding.UTF8.GetString(data);
                Pipe.Socket.ReceiveTimeout = -1;
            }
            catch (Exception ex)
            { }
            return result;
        }

        public T Receive<T>(int timeout = -1)
        {
            string json = Receive(timeout);
            if (string.IsNullOrEmpty(json)) return default(T);
            return JsonHelper.Deserialize<T>(json);
        }

        public void Close()
        {
            Pipe?.Close();
        }

        public void Dispose()
        {
            Pipe?.Dispose();
        }

        public void Disconnect()
        {
            Pipe.Disconnect();
        }
    }
}
