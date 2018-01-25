using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Entity
{
    public class G_POSITION : BaseEntity
    {
        [Primarykey]
        public string ZDID { get; set; }

        public int? ZCZT { get; set; }

        public DateTime? TIMES { get; set; }

        public int? DIRECTION { get; set; }

        public int? SPEED { get; set; }

        public decimal? LATITUDE { get; set; }

        public decimal? LONGITUDE { get; set; }

        public decimal? MILEAGE { get; set; }

        public int? HEIGHT { get; set; }

        public int? METER { get; set; }

        public decimal? OILS { get; set; }

        public int? STATE { get; set; }

        public long? ALARM { get; set; }

        public int? GQZT { get; set; }

        public int? DWZT { get; set; }

        public int? SXZT { get; set; }

        public DateTime? SXSJ { get; set; }

        public string LYDZ { get; set; }

        public string DLWZ { get; set; }

        public int WEIGHT { get; set; }
    }
}
