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
        string gender = "";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OfficialInfoRegion.Visibility = Visibility.Collapsed;
            PositionCb_load();
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

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == UsernameTb.Text).ToListAsync();
           if(user.Count==0){

           }
            this.Frame.Navigate(typeof(LogInPage));
        }
        private void MaleRBtn_Checked(object sender, RoutedEventArgs e)
        {
            gender = "M";
        }
        private void FemaleRBtn_Checked(object sender, RoutedEventArgs e)
        {
            gender = "F";
        }
        private void PositionCb_load()
        {
            PositionCb.Items.Add("Village Health Volunteer");
            PositionCb.Items.Add("Tambon Health Promoting Hospital");
            PositionCb.Items.Add("District Public Health Office");
            PositionCb.Items.Add("Povincial Public Health Office");
            PositionCb.Items.Add("Ministry of Public Health");
          //  PositionCb.SelectedIndex = PositionCb.FindStringExact("Sunday");

        }


    }
}
