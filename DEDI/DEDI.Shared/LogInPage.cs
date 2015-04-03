using System;
using System.Collections.Generic;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class LogInPage
    {
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

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
