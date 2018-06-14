
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Pipe
{
    public class Contract
    {
        private MessageTypeEnum command = MessageTypeEnum.Ready;
        public MessageTypeEnum Command { get => command; set => command = value; }

        public string Data { get; set; }

        public string Message { get; set; }

        public string NameAddress { get; set; }

        public string GroupName { get; set; }

        public string Topic { get; set; }

        public string Keys { get; set; }

        public string Tags { get; set; }

        public long ThreadID { get; set; }
    }

    public enum MessageTypeEnum
    {
        OK,
        Error,
        Connect,
        Ping,
        ReleaseBlock,
        End,
        Ready,
        Work,
    }
}
