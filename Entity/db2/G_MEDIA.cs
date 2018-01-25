
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chainway.Library.SimpleMapper;

namespace Chainway.SyncData.Entity
{
    public class G_MEDIA : BaseEntity
    {

        public string BS { get; set; }


        public byte[] DATA { get; set; }


        public int? EVENTS { get; set; }


        public string FORMATS { get; set; }


        public string JLYBH { get; set; }


        public string JLYXM { get; set; }


        public string LAT { get; set; }


        public string LON { get; set; }


        public int? SCBS { get; set; }


        public DateTime? TIMES { get; set; }


        public int? TYPE { get; set; }


        public string XYBH { get; set; }


        public string XYXM { get; set; }

        [Primarykey]
        public string ZDID { get; set; }

    }
}