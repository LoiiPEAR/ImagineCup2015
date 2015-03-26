
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
            CountryCb_load();
        }

        private async void CountryCb_load()
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("Assets");
            StorageFile file= await folder.GetFileAsync("Country_List.txt");
            string json = await Windows.Storage.FileIO.ReadTextAsync(file);
            JsonArray Countries = JsonArray.Parse(json);
            foreach (var country in Countries)
                {
                   JsonObject itemObject = country.GetObject();
                   CountryCb.Items.Add(itemObject["name"].GetString());
                }
        }
        private async void getCity(object sender, SelectionChangedEventArgs e)
        {
            string country = CountryCb.SelectedItem.ToString();
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("City_List.txt");
            string json = await Windows.Storage.FileIO.ReadTextAsync(file);
            JsonObject countries = JsonObject.Parse(json);
            if (countries.ContainsKey(country))
            {
                JsonArray cities = countries[country].GetArray();
                foreach (var city in cities) CityCb.Items.Add(city.GetString());
            }
            
            
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
            string warning_str =" is required*";
            if (FirstNameTb.Text == "")
            {
                Firstname_errorTbl.Text = "Firstname"+warning_str;
            }
            if (LastNameTb.Text == "")
            {
                Lastname_errorTbl.Text = "Lastname" + warning_str;
            }
            if (OrganizationTb.Text == "")
            {
                Organization_errorTbl.Text = "Organization name" + warning_str;
            }
            if (CountryCb.SelectedItem== "")
            {
                Country_errorTbl.Text = "Country" + warning_str;
            }
            if (PositionCb.SelectedItem == "")
            {
                Position_errorTbl.Text = "Position" + warning_str;
            }
            if (gender == "")
            {
                Gender_errorTbl.Text = "Gender" + warning_str;
            }
            if (Username_errorTbl.Text == "" && Password_errorTbl.Text == "" && Firstname_errorTbl.Text == "" && Gender_errorTbl.Text==""&&Lastname_errorTbl.Text == "" && Email_errorTbl.Text == "" && Organization_errorTbl.Text == "" && Position_errorTbl.Text == "" && Country_errorTbl.Text == "")
            {
                this.Frame.Navigate(typeof(LogInPage));
            }
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
        public static bool isEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        private void checkEmail(object sender, TextChangedEventArgs e)
        {
            bool correct = isEmail(EmailTb.Text);
            if (correct == false)
            {
                Email_errorTbl.Text = "This email is invalid.";
            }
            else Email_errorTbl.Text = "";
        }
        private async void isUsernameUsed(object sender, TextChangedEventArgs e)
        {
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == UsernameTb.Text).ToListAsync();
            if (user.Count != 0)
            {
                Username_errorTbl.Text = "This username has already taken.";
            }
            else Username_errorTbl.Text = "";
            
        }
        public static bool isPassword(string inputPW)
        {
            string strRegex = @"(?!^[0-9]*$)(?!^[a-z]*$)(?!^[A-Z]*$)^(.{8,15})$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputPW))
                return (true);
            else
                return (false);
        }
        private void checkPW(object sender, RoutedEventArgs e)
        {
            bool correct = isPassword(PasswordTb.Password);
            if (correct == false)
            {
                Password_errorTbl.Text = "8-15 characters including uppercase, lowercase and 1number.";
            }
            else Password_errorTbl.Text = "";
        }

    }
}
