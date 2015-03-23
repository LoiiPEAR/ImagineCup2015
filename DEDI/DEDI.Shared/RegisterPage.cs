using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class RegisterPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OfficialInfoRegion.Visibility = Visibility.Collapsed;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }
        
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            PersonalInfoRegion.Visibility = Visibility.Collapsed;
            OfficialInfoRegion.Visibility = Visibility.Visible;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            PersonalInfoRegion.Visibility = Visibility.Visible;
            OfficialInfoRegion.Visibility = Visibility.Collapsed;
        }

        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }
    }
}
