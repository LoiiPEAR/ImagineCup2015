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
        public int id { get; set; }
        [DataMember]
        public double Lat { get; set; }
        [DataMember]
        public double Lon { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public HealthWorker HealthWorker { get; set; }
        [DataMember]
        public string Patient { get; set; }
        [DataMember]
        public DateTime ReportTime { get; set; }
        [DataMember]
        public DateTime OcurrTime { get; set; }
        [DataMember]
        public double ProbofDisease { get; set; }
        [DataMember]
        public List<string> RiskFactor { get; set; }
        [DataMember]
        public List<string> Symptom { get; set; }


    }
}
