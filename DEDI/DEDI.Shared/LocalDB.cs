using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class LocalDB
    {
        private SQLiteAsyncConnection conn = new SQLiteAsyncConnection("dediLocal.db");
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            conn.CreateTableAsync<Disaster_Report_Local>();
            conn.CreateTableAsync<Disease_Local>();
            conn.CreateTableAsync<Disease_Report_Local>();
            conn.CreateTableAsync<Edited_Report_Local>();
            conn.CreateTableAsync<Follow_Local>();
            conn.CreateTableAsync<Health_Worker_Local>();
            conn.CreateTableAsync<Message_Local>();
            conn.CreateTableAsync<Patient_Local>();
            conn.CreateTableAsync<Reported_Symptom_Local>();
            conn.CreateTableAsync<Risk_Factor_Report_Local>();
            conn.CreateTableAsync<Symptom_Local>();

            conn.InsertAllAsync(new[]
            {
                new Health_Worker_Local {fname="Prae", lname="Charkratpahu", gender="F", dob=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,10,39,30),
                    telephone="0123456789", email="abc@a.com", organization="DEDI", username="Loii.PEAR",
                    password="z,iyd86I", position="Village Health Volunteer", latitude=13, longitude=100}
            });
        }
    }
}
