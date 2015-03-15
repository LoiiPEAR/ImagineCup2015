using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DEDI
{
    [DataContract]
    class Report
    {
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string user_report { get; set; }
    }
}
