using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
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
        
        private SQLiteAsyncConnection conn = new SQLiteAsyncConnection("localsync.db");
        private MobileServiceCollection<Bayesian_Prob, Bayesian_Prob> HWs;
        private IMobileServiceSyncTable<Bayesian_Prob> HWTable = App.MobileService.GetSyncTable<Bayesian_Prob>();
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.New)
                return;

            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("localsync.db");
                store.DefineTable<Bayesian_Prob>();
                await App.MobileService.SyncContext.InitializeAsync(store, new SyncHandler(App.MobileService));
            }
            await RefreshHW();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Bayesian_Prob selected = ListViews.Items[ListViews.SelectedIndex] as Bayesian_Prob;
            conn.DeleteAsync(selected);
        }

        private async void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            await RefreshHW();
        }

        private async void PullBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string errorString = null;

            PullBtn.Focus(FocusState.Programmatic);
            PullBtn.IsEnabled = false;

            try
            {
                await HWTable.PullAsync("id", HWTable.CreateQuery());
                await RefreshHW();
            }
            catch(MobileServicePushFailedException ex)
            {
                errorString = "Internal Push operation during pull request failed because of sync errors: " +
                    ex.PushResult.Errors.Count + " errors, message: " + ex.Message;
            }
            catch(Exception ex)
            {
                errorString = "Pull failed: " + ex.Message +
                    "\n\nIf you are still in an offline scenario, " +
                    "you can try your Pull again when connected with your Mobile Service.";
            }

            if (errorString != null)
            {
                MessageDialog d = new MessageDialog(errorString);
                await d.ShowAsync();
            }
            PullBtn.IsEnabled = true;
        }

        private async void PushBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string errorString = null;

            PushBtn.Focus(FocusState.Programmatic);
            PushBtn.IsEnabled = false;

            try
            {
                await App.MobileService.SyncContext.PushAsync();
            }
            catch (MobileServicePushFailedException ex)
            {
                errorString = "Push failed because of sync errors: " +
                    ex.PushResult.Errors.Count + " errors, message: " + ex.Message;
            }
            catch (Exception ex)
            {
                errorString = "Push failed: " + ex.Message +
                    "\n\nIf you are still in an offline scenario, " +
                    "you can try your Push again when connected with your Mobile Service.";
            }

            if (errorString != null)
            {
                MessageDialog d = new MessageDialog(errorString);
                await d.ShowAsync();
            }
            PushBtn.IsEnabled = true;
        }

        private async Task RefreshHW()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                HWs = await HWTable.OrderBy(Bayesian_Prob => Bayesian_Prob.id).ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListViews.ItemsSource = HWs;
                this.SaveBtn.IsEnabled = true;
            }
            SaveBtn.Content = ListViews.Items.Count;
        }

        private async Task UpdateHW(Bayesian_Prob hw)
        {
            await HWTable.UpdateAsync(hw);
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //var hw = new Health_Worker
            //{
            //    fname = "Prae",
            //    lname = "Charkratpahu",
            //    gender = "F",
            //    dob = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 39, 30),
            //    telephone = "0123456789",
            //    email = "abc@a.com",
            //    organization = "DEDI",
            //    username = "Loii.PEAR",
            //    password = "z,iyd86I",
            //    position = "Village Health Volunteer",
            //    latitude = 13,
            //    longitude = 100
            //};
            //await InsertHW(hw);
        }

        private async Task InsertHW(Bayesian_Prob hw)
        {
            await HWTable.InsertAsync(hw);
            HWs.Add(hw);
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
