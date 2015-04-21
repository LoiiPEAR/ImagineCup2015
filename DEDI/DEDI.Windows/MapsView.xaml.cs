using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Bing.Maps;



using Windows.UI.Popups;

using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;
using System.Net.Http;
using Windows.UI;
namespace DEDI
{
    public sealed partial class MapsView:Page
    {
        Health_Worker user;
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

       public  async void InitializeMap()
        {


            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 10;
            myMap.MapType = MapType.Road;


            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                          TimeSpan.FromSeconds(10));
            myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
            var client = new HttpClient();
            if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
            {
                var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                var serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                var jsonResponse = list as RootObject;
                user_postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
            }
            loadRF();
            loadDisaster();
            
            loadDisease();
            


        }

#if WINDOWS_APP
       private async void loadDisaster()
       {

           var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
           if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
           {
               var client = new HttpClient();
               foreach (Disaster_Report report in reports)
               {
                   var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                   var response = await client.GetAsync(Uri);
                   var result = await response.Content.ReadAsStringAsync();
                   var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                   var serializer = new DataContractJsonSerializer(typeof(RootObject));
                   var list = serializer.ReadObject(ms);
                   var jsonResponse = list as RootObject;
                   string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;

                   if (postcode == user_postcode)
                   {
                       Pushpin pushpin = new Pushpin();
                       pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                       pushpin.Name = report.id;

                       pushpin.Background = new SolidColorBrush(Colors.GreenYellow);
                       MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                       myMap.Children.Add(pushpin);
                   }
               }

           }
           else
           {
               foreach (Disaster_Report report in reports)
               {
                   Pushpin pushpin = new Pushpin();
                   pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                   pushpin.Name = report.id;

                   pushpin.Background = new SolidColorBrush(Colors.GreenYellow);
                   MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                   myMap.Children.Add(pushpin);
               }
           }

       }
       private async void loadRF()
       {

           var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
           if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
           {
               var client = new HttpClient();
               foreach (Risk_Factor_Report report in reports)
               {
                   var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                   var response = await client.GetAsync(Uri);
                   var result = await response.Content.ReadAsStringAsync();
                   var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                   var serializer = new DataContractJsonSerializer(typeof(RootObject));
                   var list = serializer.ReadObject(ms);
                   var jsonResponse = list as RootObject;
                   string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                   if (postcode == user_postcode)
                   {
                       Pushpin pushpin = new Pushpin();
                       pushpin.Tapped += new TappedEventHandler(pushpinTapped);

                       pushpin.Background = new SolidColorBrush(Colors.Blue);
                       pushpin.Name = report.id;
                       MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                       myMap.Children.Add(pushpin);
                   }
               }

           }
           else
           {
               foreach (Risk_Factor_Report report in reports)
               {
                   Pushpin pushpin = new Pushpin();
                   pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                   pushpin.Name = report.id;

                   pushpin.Background = new SolidColorBrush(Colors.Blue);
                   MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                   myMap.Children.Add(pushpin);
               }
           }
       }

       private async void loadDisease()
       {
           var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
           if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
           {
               var client = new HttpClient();
               foreach (Disease_Report report in reports)
               {
                   var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                   var response = await client.GetAsync(Uri);
                   var result = await response.Content.ReadAsStringAsync();
                   var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                   var serializer = new DataContractJsonSerializer(typeof(RootObject));
                   var list = serializer.ReadObject(ms);
                   var jsonResponse = list as RootObject;
                   string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                   if (postcode == user_postcode)
                   {
                       Pushpin pushpin = new Pushpin();
                       pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                       pushpin.Name = report.id;

                       pushpin.Background = new SolidColorBrush(Colors.DeepPink);
                       MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                       myMap.Children.Add(pushpin);
                   }
               }

           }
           else
           {
               foreach (Disease_Report report in reports)
               {
                   Pushpin pushpin = new Pushpin();
                   pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                   pushpin.Name = report.id;

                   pushpin.Background = new SolidColorBrush(Colors.DeepPink);
                   MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                   myMap.Children.Add(pushpin);
               }
           }

       }



       private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
       {
           MessageDialog dialog;
           var disaster_reports = await App.MobileService.GetTable<Disaster_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
           if (disaster_reports.Count > 0)
           {
               string Title = disaster_reports[0].disaster;
               string Content = disaster_reports[0].description + "\n" + disaster_reports[0].ocurred_time.Date;
               dialog = new MessageDialog(Content, Title);
               await dialog.ShowAsync();

           }
           var disease_reports = await App.MobileService.GetTable<Disease_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
           if (disease_reports.Count > 0)
           {
               string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
               string Content = disease_reports[0].description + "\n" + disease_reports[0].ocurred_time.Date;
               dialog = new MessageDialog(Content, Title);
               await dialog.ShowAsync();

           }
           var rf_reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
           if (rf_reports.Count > 0)
           {
               string Title = rf_reports[0].risk_factor;
               string Content = rf_reports[0].description + "\n" + rf_reports[0].ocurred_time.Date;
               dialog = new MessageDialog(Content, Title);
               await dialog.ShowAsync();

           }


       }
#endif  
       private void backButton_Click(object sender, RoutedEventArgs e)
       {
           this.Frame.Navigate(typeof(HomePage), user);
       }
    }
}
