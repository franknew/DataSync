
using Chainway.Library.SimpleMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chainway.SyncData.Entity
{
    public class D_MEDIA : BaseEntity
    {

        public string COACH_NAME { get; set; }


        public string STUDENT_NAME { get; set; }


        public string COACH_NO { get; set; }


        public string STUDENT_NO { get; set; }


        public string URLS { get; set; }


        public byte[] DATAS { get; set; }

        [Primarykey]
        public string ID { get; set; }


        public string CAR_ID { get; set; }


        public DateTime? TIMES { get; set; }


        public int? TYPES { get; set; }


        public string FORMATS { get; set; }


        public int? EVENTS { get; set; }


        public string LON { get; set; }


        public string LAT { get; set; }

        public string ZDID { get; set; }

    }
}