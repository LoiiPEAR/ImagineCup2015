using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class RegisterPersonalInfoPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }
        
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterOfficialInfoPage));
        }
    }
}
