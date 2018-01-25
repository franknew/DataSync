using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Entity
{
    public class Test : BaseEntity
    {
        [Primarykey]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
