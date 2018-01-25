using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.BLL
{
    public class T_Sync_Temp
    {
        public long ID { get; set; }
        public string TableName { get; set; }
        public string Address { get; set; }
        public string Tags { get; set; }
        public string Data { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Topic { get; set; }
        public string Group { get; set; }
    }
}
