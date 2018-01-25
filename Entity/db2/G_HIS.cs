
using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Entity
{
    public class G_HIS : BaseEntity
    {

        public string ALARM { get; set; }


        public int? DIRECTION { get; set; }


        public string DLWZ { get; set; }


        public int? DWZT { get; set; }


        public int? HEIGHT { get; set; }


        public string LATITUDE { get; set; }


        public string LONGITUDE { get; set; }


        public int? METER { get; set; }


        public string MILEAGE { get; set; }


        public string OILS { get; set; }


        public int? SPEED { get; set; }


        public string STATE { get; set; }

        [Primarykey]
        public DateTime? TIMES { get; set; }


        public string TMILEAGE { get; set; }


        public int? TYPE { get; set; }

        [Primarykey]
        public string ZDID { get; set; }

    }
}