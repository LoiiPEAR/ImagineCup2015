using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;

namespace DEDI
{
    class Disaster_Report
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "hw_id")]
        public string hw_id { get; set; }
        [JsonProperty(PropertyName = "disaster")]
        public string disaster { get; set; }
        [JsonProperty(PropertyName = "reported_time")]
        public DateTime reported_time { get; set; }
        [JsonProperty(PropertyName = "ocurred_time")]
        public DateTime ocurred_time { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Edited_Report
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "original_disease_report_id")]
        public string original_disease_report_id { get; set; }
        [JsonProperty(PropertyName = "edited_disease_report_id")]
        public string edited_disease_report_id { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Follow
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "following_hw_id")]
        public string following_hw_id { get; set; }
        [JsonProperty(PropertyName = "follower_hw_id")]
        public string follower_hw_id { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Message
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "hw_sender_id")]
        public string hw_sender_id { get; set; }
        [JsonProperty(PropertyName = "hw_receiver_id")]
        public string hw_receiver_id { get; set; }
        [JsonProperty(PropertyName = "topic")]
        public string topic { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string content { get; set; }
        [JsonProperty(PropertyName = "sent_time")]
        public DateTime sent_time { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }


    class Patient
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string gender { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string address { get; set; }
        [JsonProperty(PropertyName = "telephone")]
        public string telephone { get; set; }
        [JsonProperty(PropertyName = "weight")]
        public double weight { get; set; }
        [JsonProperty(PropertyName = "height")]
        public double height { get; set; }
        [JsonProperty(PropertyName = "dob")]
        public DateTime dob { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Health_Worker
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName="fname")]
        public string fname { get; set; }
        [JsonProperty(PropertyName = "lname")]
        public string lname { get; set; }
        [JsonProperty(PropertyName = "gender")]
        public string gender { get; set; }
        [JsonProperty(PropertyName = "dob")]
        public DateTime dob { get; set; }
        [JsonProperty(PropertyName = "telephone")]
        public string telephone { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }
        [JsonProperty(PropertyName = "organization")]
        public string organization { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }
        [JsonProperty(PropertyName = "position")]
        public string position { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Disease_Report
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }
        [JsonProperty(PropertyName = "disease_id")]
        public string disease_id { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "hw_id")]
        public string hw_id { get; set; }
        [JsonProperty(PropertyName = "patient_id")]
        public string patient_id { get; set; }
        [JsonProperty(PropertyName = "reported_time")]
        public DateTime reported_time { get; set; }
        [JsonProperty(PropertyName = "occurred_time")]
        public DateTime ocurred_time { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Disease
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "disease")]
        public string disease { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Reported_Symptom
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "disease_report_id")]
        public string disease_report_id { get; set; }
        [JsonProperty(PropertyName = "symptom_id")]
        public string symptom_id { get; set; }
        [JsonProperty(PropertyName = "intensity_id")]
        public string intensity_id { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    
    class Risk_Factor_Report
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }
        [JsonProperty(PropertyName = "hw_id")]
        public string hw_id { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "risk_factor")]
        public string risk_factor { get; set; }
        [JsonProperty(PropertyName = "reported_time")]
        public DateTime reported_time { get; set; }
        [JsonProperty(PropertyName = "occurred_time")]
        public DateTime ocurred_time { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Symptom
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "symptom")]
        public string symptom { get; set; }
        [JsonProperty(PropertyName = "body_part")]
        public string body_part { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
