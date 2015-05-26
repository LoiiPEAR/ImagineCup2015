

using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using System;
using System.Net.Http;
using Windows.Storage.Streams;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DEDI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsView
    {
        private Health_Worker user;

        string user_postcode;
        public MapsView()
        {
            this.InitializeComponent();
            InitializeMap();
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            
        }

       public async void InitializeMap()
        {
            
            myMap.MapServiceToken = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 10;


            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            Geocoordinate myGeocoordinate = myGeoposition.Coordinate;

            myMap.Center = myGeocoordinate.Point;
            loadRF();
            loadDisaster();

            loadDisease();
           

           

        }

       private async void loadDisease()
       {
          var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
          //if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
          //{
          //    var client = new HttpClient();
              foreach (Disease_Report report in reports)
              {
              //    var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
              //    var response = await client.GetAsync(Uri);
              //    var result = await response.Content.ReadAsStringAsync();
              //    var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
              //    var serializer = new DataContractJsonSerializer(typeof(RootObject));
              //    var list = serializer.ReadObject(ms);
              //    var jsonResponse = list as RootObject;
              //    string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                  //if (postcode == user_postcode)
                  //{
                      AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "p");

                  //}
              }
          //}
       }

       private async void loadDisaster()
       {

           var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
           //if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
           //{
           //    var client = new HttpClient();
               foreach (Disaster_Report report in reports)
               {
                   //var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                   //var response = await client.GetAsync(Uri);
                   //var result = await response.Content.ReadAsStringAsync();
                   //var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                   //var serializer = new DataContractJsonSerializer(typeof(RootObject));
                   //var list = serializer.ReadObject(ms);
                   //var jsonResponse = list as RootObject;
                   //string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;

                   //if (postcode == user_postcode)
                   //{
                       AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id,"g");

                   //}
               }

           //}
           //else
           //{
           //    foreach (Disaster_Report report in reports)
           //    {
           //        AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id,"g");
           //    }
           //}

       }

       private async void loadRF()
       {
           var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
           //if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
           //{
           //    var client = new HttpClient();
               foreach (Risk_Factor_Report report in reports)
               {
                   //var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                   //var response = await client.GetAsync(Uri);
                   //var result = await response.Content.ReadAsStringAsync();
                   //var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                   //var serializer = new DataContractJsonSerializer(typeof(RootObject));
                   //var list = serializer.ReadObject(ms);
                   //var jsonResponse = list as RootObject;
                   //string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                   //if (postcode == user_postcode)
                   //{
                       AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id,"b");

                   //}
               }

           //}
           //else
           //{
           //    foreach (Risk_Factor_Report report in reports)
           //    {
           //        AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id,"b");

           //    }
           //}
       }
        public void AddPushpin(BasicGeoposition location, string text,string color)
        {
           

            var pin = new Grid()
            {
                Width = 30,
                Height = 30,
                Margin = new Windows.UI.Xaml.Thickness(-12),
                Name = text
            };

            if (color == "g")
            {
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.GreenYellow),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });
            }
            if (color == "b")
            {
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Blue),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });
            }
            if (color == "p")
            {
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.DeepPink),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });
            }
            
           
            MapControl.SetLocation(pin, new Geopoint(location));
            pin.Tapped += pushpinTapped;
            myMap.Children.Add(pin);
        }

        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog;
            var disaster_reports = await App.MobileService.GetTable<Disaster_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
            if (disaster_reports.Count > 0)
            {
                string Title = disaster_reports[0].disaster;
                string Content = disaster_reports[0].description + "\n" + disaster_reports[0].occurred_time.Date;
                dialog = new MessageDialog(Content, Title);
                await dialog.ShowAsync();

            }
            var d = await App.MobileService.GetTable<Disease_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
            if (d.Count > 0)
            {
                //string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
                string Title = "Probability of this disease report";
                string Content = "Cholera: " + d[0].cholera + "\n" + "Shigella: " + d[0].shigella + "\n" + "Salmoella: " + d[0].salmonella + "\n" + "Rotavirus: " + d[0].rotavirus + "\n" + "Others: " + d[0].others + "\n\n" + d[0].occurred_time.Date;
                dialog = new MessageDialog(Content, Title);
                await dialog.ShowAsync();

            }
            var rf_reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
            if (rf_reports.Count > 0)
            {
                string Title = rf_reports[0].risk_factor;
                string Content = rf_reports[0].description + "\n" + rf_reports[0].occurred_time.Date;
                dialog = new MessageDialog(Content, Title);
                await dialog.ShowAsync();

            }

        }

        private void backButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
        }

        
    }
}
