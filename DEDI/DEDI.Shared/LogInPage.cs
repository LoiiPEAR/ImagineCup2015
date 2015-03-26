using System;
using System.Collections.Generic;
using System.Text;
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
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == UsernameTb.Text &&hw.password == PasswordTb.Password).ToListAsync();
            if (user.Count != 0)
            {
                this.Frame.Navigate(typeof(HomePage),user[0]);
            }
            else{
                errorTbl.Text = "Username or password is incorrect.";
            }
        }
        private void password_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        
    }
}
