using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using SQLite;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;

namespace DEDI
{

   

    //public class JSONB
    //{
    //    public List<Bayesian_Prob> BayesianProb { get; set; }
    //}
    public class Bayesian_Prob
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "Cholera")]
        public double Cholera { get; set; }
        [JsonProperty(PropertyName = "Eyes")]
        public string Eyes { get; set; }
        [JsonProperty(PropertyName = "Fever")]
        public string Fever { get; set; }
        [JsonProperty(PropertyName = "Food")]
        public string Food { get; set; }
        [JsonProperty(PropertyName = "Nature_of_Stool")]
        public string Nature_of_Stool { get; set; }
        [JsonProperty(PropertyName = "Others")]
        public double Others { get; set; }
        [JsonProperty(PropertyName = "Rotavirus")]
        public double Rotavirus { get; set; }
        [JsonProperty(PropertyName = "Samonella")]
        public double Samonella { get; set; }
        [JsonProperty(PropertyName = "Shigella")]
        public double Shigella { get; set; }
        [JsonProperty(PropertyName = "Skin_Temperature")]
        public string Skin_Temperature { get; set; }
        [JsonProperty(PropertyName = "Skin_Turgor")]
        public string Skin_Turgor { get; set; }
        [JsonProperty(PropertyName = "Stool_Frequency_per_Day")]
        public string Stool_Frequency_per_Day { get; set; }
        [JsonProperty(PropertyName = "Stool_Type")]
        public string Stool_Type { get; set; }
        [JsonProperty(PropertyName = "Thirst")]
        public string Thirst { get; set; }
        [JsonProperty(PropertyName = "Urine_Output")]
        public string Urine_Output { get; set; }
        [JsonProperty(PropertyName = "Vomiting")]
        public string Vomiting { get; set; }
        [JsonProperty(PropertyName = "Water")]
        public string Water { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }
    }

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

    class Disaster_Report_View
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public ImageSource icon { get; set; }
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
    class Report
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public ImageSource icon { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "hw_id")]
        public string hw_id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }
        [JsonProperty(PropertyName = "reported_time")]
        public DateTime reported_time { get; set; }
        [JsonProperty(PropertyName = "ocurred_time")]
        public DateTime ocurred_time { get; set; }
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
        [JsonProperty(PropertyName = "status")]
        public string status { get; set; }

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
        [JsonProperty(PropertyName = "cholera")]
        public double cholera { get; set; }
        [JsonProperty(PropertyName = "shigella")]
        public double shigella { get; set; }
        [JsonProperty(PropertyName = "rotavirus")]
        public double rotavirus { get; set; }
        [JsonProperty(PropertyName = "salmonella")]
        public double salmonella { get; set; }
        [JsonProperty(PropertyName = "others")]
        public double others { get; set; }
        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
        [JsonProperty(PropertyName = "prob_id")]
        public string prob_id { get; set; }

        [Version]
        public string Version { get; set; }
    }

    class Disease_Report_View
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public ImageSource icon { get; set; }
        [JsonProperty(PropertyName = "latitude")]
        public double latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double longitude { get; set; }
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
        [JsonProperty(PropertyName = "Cholera")]
        public double cholera { get; set; }
        [JsonProperty(PropertyName = "Shigella")]
        public double shigella { get; set; }
        [JsonProperty(PropertyName = "rotavirus")]
        public double rotavirus { get; set; }
        [JsonProperty(PropertyName = "simonelle")]
        public double simolnelle { get; set; }
        [JsonProperty(PropertyName = "others")]
        public double others { get; set; }
        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [Version]
        public string Version { get; set; }

        public string type { get; set; }
    }
       class Reported_Symptom
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "disease_report_id")]
        public string disease_report_id { get; set; }
        [JsonProperty(PropertyName = "symptom")]
        public string symptom { get; set; }
        [JsonProperty(PropertyName = "intensity")]
        public string intensity { get; set; }

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

    class Risk_Factor_Report_View
    {
        [PrimaryKey]
        public string id { get; set; }
        [JsonProperty(PropertyName = "icon")]
        public ImageSource icon { get; set; }
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
