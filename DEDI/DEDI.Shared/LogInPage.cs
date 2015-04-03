using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class LogInPage
    {
        
        private SQLiteAsyncConnection conn = new SQLiteAsyncConnection("dediLocal.db");
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //conn.CreateTableAsync<Disaster_Report_Local>();
            //conn.CreateTableAsync<Disease_Local>();
            //conn.CreateTableAsync<Disease_Report_Local>();
            //conn.CreateTableAsync<Edited_Report_Local>();
            //conn.CreateTableAsync<Follow_Local>();
            await conn.CreateTableAsync<Health_Worker_Local>();
            //conn.CreateTableAsync<Message_Local>();
            //conn.CreateTableAsync<Patient_Local>();
            //conn.CreateTableAsync<Reported_Symptom_Local>();
            //conn.CreateTableAsync<Risk_Factor_Report_Local>();
            //conn.CreateTableAsync<Symptom_Local>();

            //await conn.InsertAllAsync(new[]
            //{
            //    new Health_Worker_Local {id="AAA111", fname="Prae", lname="Charkratpahu", gender="F", dob=new DateTime(DateTime.Today.Year,DateTime.Today.Month,DateTime.Today.Day,10,39,30),
            //        telephone="0123456789", email="abc@a.com", organization="DEDI", username="Loii.PEAR",
            //        password="z,iyd86I", position="Village Health Volunteer", latitude=13, longitude=100}
            //});

            //var getAll = await conn.Table<Health_Worker_Local>().Where(u=>u.fname == "Prae").FirstOrDefaultAsync();
            //UsernameTb.Text = getAll.fname;
        }

        private void RegisterHL_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private async void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            SignInBtn.IsEnabled = false;
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == UsernameTb.Text && hw.password == ComputeMD5(PasswordTb.Password)).ToListAsync();
            if (user.Count != 0)
            {
                
                this.Frame.Navigate(typeof(HomePage),user[0]);
                
            }
            else{
                errorTbl.Text = "Username or password is incorrect.";
                SignInBtn.IsEnabled = true;
            }
        }
        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        
    }
}
