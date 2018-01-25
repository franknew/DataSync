using SOAFramework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Pipe
{
    public class PipeHandler
    {
        Pipe _pipe = null;
        public PipeHandler(Pipe pipe)
        {
            _pipe = pipe;
        }

        public void Send(string data)
        {
            var list = Encoding.UTF8.GetBytes(data).ToList();
            list.Add(0);
            _pipe.Send(list.ToArray());
        }

        public void Send(object obj)
        {
            string json = JsonHelper.Serialize(obj);
            Send(json);
        }

        public string Receive(int timeout = -1)
        {
            string result = null;
            if (timeout > 0) _pipe.Socket.ReceiveTimeout = timeout * 1000;
            byte[] data = new byte[1024 * 100];
            _pipe.Receive(data);
            result = Encoding.UTF8.GetString(data);
            _pipe.Socket.ReceiveTimeout = -1;
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
            _pipe?.Close();
        }
    }
}
