using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DEDI
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }
        [DataMember]
        public string LocationName { get; set; }
    }
}
