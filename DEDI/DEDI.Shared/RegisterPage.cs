
using Bing.Maps;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
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
            InitializeMap();
            // Set the MinDate and MaxDate.
            DOBDpk.MinYear = new DateTime(1985, 6, 20);
            DOBDpk.MaxYear = DateTime.Today;
            
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
           
            if (PositionCb.SelectedItem == "")
            {
                Position_errorTbl.Text = "Position" + warning_str;
            }
            if (gender == "")
            {
                Gender_errorTbl.Text = "Gender" + warning_str;
            }
            if (EmailTbl.Text == "")
            {
                Email_errorTbl.Text = "Email" + warning_str;
            }
            if (Username_errorTbl.Text == "" && Password_errorTbl.Text == "" && Firstname_errorTbl.Text == "" && Gender_errorTbl.Text==""&&Lastname_errorTbl.Text == "" && Email_errorTbl.Text == "" && Organization_errorTbl.Text == "" && Position_errorTbl.Text == "" )
            {
                //ComboBoxItem posItem = (ComboBoxItem)PositionCb.SelectedItem;
                string pos_value = PositionCb.SelectedItem.ToString();
                Health_Worker user = new Health_Worker() { 
                fname = FirstNameTb.Text,
                lname = LastNameTb.Text,
                password = PasswordTb.Password,
                username =UsernameTb.Text,
                position = pos_value,
                organization = OrganizationTb.Text,
                latitude = Bing.Maps.MapLayer.GetPosition(pin).Latitude,
                longitude = Bing.Maps.MapLayer.GetPosition(pin).Longitude,
                email = EmailTb.Text,
                telephone = TelTb.Text,
                dob = DOBDpk.Date.UtcDateTime,
                gender = this.gender
                };
                IMobileServiceTable<Health_Worker> hwTable =  App.MobileService.GetTable<Health_Worker>();
                await hwTable.InsertAsync(user);
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
        DraggablePin pin;
        public async void InitializeMap()
        {


            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 10;
            myMap.MapType = MapType.Road;


            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                          TimeSpan.FromSeconds(10));
            myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
            pin = new DraggablePin(myMap);
            //Set the location of the pin to the center of the map.
            Bing.Maps.MapLayer.SetPosition(pin, myMap.Center);

            //Set the pin as draggable.
            pin.Draggable = true;

            //Attach to the drag event of the pin.
            pin.DragEnd += Pin_Dragged;
           
            //Add the pin to the map.
            myMap.Children.Add(pin);
            myMap.PointerPressedOverride+=myMap_PointerPressedOverride;
            var client = new HttpClient();
            Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
            var list = serializer.ReadObject(ms);
            Response jsonResponse = list as Response;
            AddressTbl.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
        }

        private async void myMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            var pointerPosition = e.GetCurrentPoint(((Map)sender));

            Bing.Maps.Location location = null;

            //Convert the point pixel to a Location coordinate
            if (((Map)sender).TryPixelToLocation(pointerPosition.Position, out location))
            {
                Bing.Maps.MapLayer.SetPosition(pin, location);
                var client = new HttpClient();
                Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + location.Latitude + "," + location.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                var list = serializer.ReadObject(ms);
                Response jsonResponse = list as Response;
                ms.Flush();
                if (jsonResponse.ResourceSets[0].EstimatedTotal != 0)
                    AddressTbl.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
            }
        }

       

        

        private async void Pin_Dragged(Bing.Maps.Location obj)
        {
            
            var client = new HttpClient();
            Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + obj.Latitude + "," + obj.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
            var list = serializer.ReadObject(ms);
            Response jsonResponse = list as Response;
            ms.Flush();
            if (jsonResponse.ResourceSets[0].EstimatedTotal!=0)
            AddressTbl.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
        }

        //private async void pushpin_DragLeave(object sender, DragEventArgs e)
        //{
        //    Pushpin pin = (Pushpin)sender;
        //     var client = new HttpClient();
        //     //Get the mouse click coordinates
        //     var mousePosition = e.GetPosition(this);
        //     //Convert the mouse coordinates to a locatoin on the map
        //     Location pinLocation = m
        //    Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + p.X+ "," + p.Y+ "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
        //    var response = await client.GetAsync(uri);
        //    var result = await response.Content.ReadAsStringAsync();
        //    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
        //    var list = serializer.ReadObject(ms);
        //    Response jsonResponse = list as Response;
        //    AddressTbl.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
        //}
       

        

        

    }
}
