using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DEDI
{
    class SQLite_DB
    {

    }

    public class Disaster_Report_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("disaster")]
        public string disaster { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("occurred_time")]
        public DateTime occurred_time { get; set; }

        [Column("reported_time")]
        public DateTime reported_time { get; set; }

        [Column("latitude")]
        public double latitude { get; set; }

        [Column("longitude")]
        public double longitude { get; set; }

        [Column("hw_id")]
        public string hw_id { get; set; }
    }

    public class Disease_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("disease")]
        public string disease { get; set; }
    }

    public class Disease_Report_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("occurred_time")]
        public DateTime occurred_time { get; set; }

        [Column("reported_time")]
        public DateTime reported_time { get; set; }

        [Column("latitude")]
        public double latitude { get; set; }

        [Column("longitude")]
        public double longitude { get; set; }

        [Column("hw_id")]
        public string hw_id { get; set; }

        [Column("disease_id")]
        public string disease_id { get; set; }

        [Column("patient_id")]
        public string patient_id { get; set; }
    }

    public class Edited_Report_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("original_disease_report_id")]
        public string original_disease_report_id { get; set; }

        [Column("edited_disease_report_id")]
        public string edited_disease_report_id { get; set; }
    }

    public class Follow_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("follower_hw_id")]
        public string follower_hw_id { get; set; }

        [Column("following_hw_id")]
        public string following_hw_id { get; set; }
    }

    public class Health_Worker_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("fname")]
        public string fname { get; set; }

        [Column("lname")]
        public string lname { get; set; }

        [Column("gender")]
        public string gender { get; set; }

        [Column("dob")]
        public DateTime dob { get; set; }

        [Column("telephone")]
        public string telephone { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("organization")]
        public string organization { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("position")]
        public string position { get; set; }

        [Column("latitude")]
        public double latitude { get; set; }

        [Column("longitude")]
        public double longitude { get; set; }
    }

    public class Message_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("topic")]
        public string topic { get; set; }

        [Column("content")]
        public string content { get; set; }

        [Column("sent_time")]
        public DateTime sent_time { get; set; }

        [Column("hw_sender_id")]
        public string hw_sender_id { get; set; }

        [Column("hw_receiver_id")]
        public string hw_receiver_id { get; set; }
    }

    class Reported_Symptom_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("disease_report_id")]
        public string disease_report_id { get; set; }

        [Column("symptom_id")]
        public string symptom_id { get; set; }

        [Column("intensity_id")]
        public string intensity_id { get; set; }
    }

    class Risk_Factor_Report_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("latitude")]
        public double latitude { get; set; }

        [Column("longitude")]
        public double longitude { get; set; }

        [Column("hw_id")]
        public string hw_id { get; set; }

        [Column("description")]
        public string description { get; set; }

        [Column("risk_factor")]
        public string risk_factor { get; set; }

        [Column("reported_time")]
        public DateTime reported_time { get; set; }

        [Column("ocurred_time")]
        public DateTime ocurred_time { get; set; }
    }

    class Symptom_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("symptom")]
        public string symptom { get; set; }

        [Column("body_part")]
        public string body_part { get; set; }
    }

    class Patient_Local
    {
        [PrimaryKey, Column("id")]
        public string id { get; set; }

        [Column("gender")]
        public string gender { get; set; }

        [Column("address")]
        public string address { get; set; }

        [Column("telephone")]
        public string telephone { get; set; }

        [Column("weight")]
        public double weight { get; set; }

        [Column("height")]
        public double height { get; set; }

        [Column("dob")]
        public DateTime dob { get; set; }

        [Column("name")]
        public string name { get; set; }
    }
}
