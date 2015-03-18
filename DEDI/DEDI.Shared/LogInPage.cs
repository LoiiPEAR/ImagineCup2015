using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
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
            this.Frame.Navigate(typeof(RegisterPersonalInfoPage));
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
    }
}
