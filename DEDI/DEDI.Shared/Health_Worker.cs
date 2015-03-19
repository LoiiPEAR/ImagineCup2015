using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DEDI
{
 
    class HealthWorker
    {
       
        public int id { get; set; }
       [JsonProperty(PropertyName = "fname")]
        public string Firstname { get; set; }
       [JsonProperty(PropertyName = "lname")]
       public string Lastname { get; set; }
       [JsonProperty(PropertyName = "gender")]
       public string Gender { get; set; }
       [JsonProperty(PropertyName = "DOB")]
       public DateTime DateofBirth { get; set; }
       [JsonProperty(PropertyName = "telephone")]
       public string Telephone { get; set; }
       [JsonProperty(PropertyName = "email")]
       public string Email { get; set; }

    }
}
