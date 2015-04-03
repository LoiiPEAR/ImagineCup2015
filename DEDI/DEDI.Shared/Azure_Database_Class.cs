using System;
using System.Collections.Generic;
using System.Text;

namespace DEDI
{
    class Disaster_Report
    {
        public string id { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string description { get; set; }

        public string hw_id { get; set; }

        public string disaster { get; set; }

        public DateTime reported_time { get; set; }

        public DateTime ocurred_time { get; set; }
    
    }

    class Edited_Report
    {
        public string id { get; set; }

        public string original_disease_report_id { get; set; }

        public string edited_disease_report_id { get; set; }
    }

    class Follow
    {
        public string id { get; set; }

        public string following_hw_id { get; set; }

        public string follower_hw_id { get; set; }
    }

    class Message
    {
        public string id { get; set; }

        public string hw_sender_id { get; set; }

        public string hw_receiver_id { get; set; }

        public string topic { get; set; }

        public string content { get; set; }

        public DateTime sent_time { get; set; }
    }

    class Patient
    {
        public string id { get; set; }

        public string gender { get; set; }

        public string address { get; set; }

        public string telephone { get; set; }

        public double weight { get; set; }

        public double height { get; set; }

        public DateTime dob { get; set; }

        public string name { get; set; }
    }

    class Health_Worker
    {

        public string id { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public string organization { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string position { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    class Disease_Report
    {

        public string id { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string disease_id { get; set; }

        public string description { get; set; }

        public string hw_id { get; set; }

        public string patient_id { get; set; }

        public DateTime reported_time { get; set; }

        public DateTime ocurred_time { get; set; }

    }

    class Disease
    {
        public string id { get; set; }

        public string disease { get; set; }
    }

    class Reported_Symptom
    {
        public string id { get; set; }

        public string disease_report_id { get; set; }

        public string symptom_id { get; set; }

        public string intensity_id { get; set; }
    }

    class Risk_Factor
    {
        public string id { get; set; }

        public string risk_factor { get; set; }
    }

    class Risk_Factor_Intensity
    {
        public string id { get; set; }

        public string rf_id { get; set; }

        public int level { get; set; }

        public string description { get; set; }
    }

    class Risk_Factor_Report
    {
        public string id { get; set; }
      
        public double latitude { get; set; }

        public double longitude { get; set; }

        public string hw_id { get; set; }

        public string description { get; set; }

        public string rf_id { get; set; }

        public string intensity_id { get; set; }

        public DateTime reported_time { get; set; }

        public DateTime ocurred_time { get; set; }
    }

    class Symptom
    {
        public string id { get; set; }

        public string symptom { get; set; }

        public string body_part { get; set; }
    }
}
