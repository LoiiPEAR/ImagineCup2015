#if WINDOWS_APP
    using Bing.Maps;
#endif
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Windows.Data.Json;
using Windows.UI.Xaml.Input;


namespace DEDI
{
    public sealed partial class HomePage
    {
        private Health_Worker user;
#if WINDOWS_APP
        Map myMap;
#endif
        public HomePage()
        {
            this.InitializeComponent();
           
        }

        private void emergencyBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MessageViewPage), user);
        }

        private async void loadReports()
        {
            ListView lv = FindChildControl<ListView>(ReportsSection, "reportList") as ListView;
                
            var disasterReports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
            var riskReports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
            List<Report> all = new List<Report>();
            foreach (Disaster_Report disasteritem in disasterReports)
            {
                all.Add(new Report
                {
                    name = disasteritem.disaster,
                    hw_id = disasteritem.hw_id,
                    latitude = disasteritem.latitude,
                    longitude = disasteritem.longitude,
                    description = disasteritem.description,
                    reported_time = disasteritem.reported_time,
                    ocurred_time = disasteritem.ocurred_time
                });
            }
            foreach (Risk_Factor_Report riskitem in riskReports)
            {
                all.Add(new Report
                {
                    name = riskitem.risk_factor,
                    hw_id = riskitem.hw_id,
                    latitude = riskitem.latitude,
                    longitude = riskitem.longitude,
                    description = riskitem.description,
                    reported_time = riskitem.reported_time,
                    ocurred_time = riskitem.ocurred_time
                });
            }
            all = all.OrderByDescending(o => o.ocurred_time).ToList();
            lv.ItemsSource = all;
        }

        private async void loadData()
        {
            try
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                               TimeSpan.FromSeconds(10));
                var client = new HttpClient();
                Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                RootObject jsonResponse = list as RootObject;
            
                
                
                TextBlock FirstNameTbl = FindChildControl<TextBlock>(ProfileSection, "FirstNameTbl") as TextBlock;
                FirstNameTbl.Text = user.fname;
                TextBlock LastNameTbl = FindChildControl<TextBlock>(ProfileSection, "LastNameTbl") as TextBlock;
                LastNameTbl.Text = user.lname;
                TextBlock LocationTbl = FindChildControl<TextBlock>(ProfileSection, "LocationTbl") as TextBlock;
                LocationTbl.Text = jsonResponse.results[0].formatted_address;

                var following = await App.MobileService.GetTable<Follow>().Where(p => p.following_hw_id == user.id).ToListAsync();
                TextBlock followingTbl = FindChildControl<TextBlock>(ProfileSection, "followingTbl") as TextBlock;
                followingTbl.Text = following.Count + "";

                var follower = await App.MobileService.GetTable<Follow>().Where(p => p.follower_hw_id == user.id).ToListAsync();
                TextBlock followerTbl = FindChildControl<TextBlock>(ProfileSection, "followersTbl") as TextBlock;
                followerTbl.Text = follower.Count + "";

                var disaster_rp = await App.MobileService.GetTable<Disaster_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                var disease_rp = await App.MobileService.GetTable<Disease_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                var rf_rp = await App.MobileService.GetTable<Risk_Factor_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                TextBlock myreport = FindChildControl<TextBlock>(ProfileSection, "myreportsTbl") as TextBlock;
                int no_myreport = disaster_rp.Count + disease_rp.Count + rf_rp.Count;
                myreport.Text = no_myreport + "";

                //myMap = FindChildControl<Map>(MapSection, "myMap") as Map;
                //myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                //myMap.ZoomLevel = 10;
                //myMap.MapType = MapType.Road;
                //myMap.Width = 420;
                //myMap.Height = 480;
                //myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                //loadRF();
                //loadDisaster();
                //loadReports();
            }
            catch (MobileServiceInvalidOperationException e){

            }
        }
        

            
            
        
      private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        //public static T DeserializeJSon<T>(string jsonString)
        //{
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        //    MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        //    T obj = (T)ser.ReadObject(stream);
        //    return obj;
        //}
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            loadData();
           
        }



        private void CreateReportBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage),user);
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashBoard),user);
        }
        private void SignoutBtn_click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }

        private void GoToMapBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapsView), user);
        }

        private void GoToReportBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReportsView), user);
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage), user);
        }
        private void NearbyBtn_Click(object sender, RoutedEventArgs e)
        {
            Image allBG = FindChildControl<Image>(NotiSection, "allBG") as Image;
            allBG.Visibility = Visibility.Collapsed;
            Image nearbyBG = FindChildControl<Image>(NotiSection, "nearbyBG") as Image;
            nearbyBG.Visibility = Visibility.Visible;
            ScrollViewer AllScrollView = FindChildControl<ScrollViewer>(NotiSection, "AllScrollView") as ScrollViewer;
            AllScrollView.Visibility = Visibility.Collapsed;
            ScrollViewer NearbyScrollView = FindChildControl<ScrollViewer>(NotiSection, "NearbyScrollView") as ScrollViewer;
            NearbyScrollView.Visibility = Visibility.Visible;
        }
        private void AllBtn_Click(object sender, RoutedEventArgs e)
        {
            Image allBG = FindChildControl<Image>(NotiSection, "allBG") as Image;
            allBG.Visibility = Visibility.Visible;
            Image nearbyBG = FindChildControl<Image>(NotiSection, "nearbyBG") as Image;
            nearbyBG.Visibility = Visibility.Collapsed;
            ScrollViewer AllScrollView = FindChildControl<ScrollViewer>(NotiSection, "AllScrollView") as ScrollViewer;
            AllScrollView.Visibility = Visibility.Visible;
            ScrollViewer NearbyScrollView = FindChildControl<ScrollViewer>(NotiSection, "NearbyScrollView") as ScrollViewer;
            NearbyScrollView.Visibility = Visibility.Collapsed;
        }
        private async void loadDisaster()
        {
#if WINDOWS_APP
            var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
            foreach (Disaster_Report report in reports)
            {
                Pushpin pushpin = new Pushpin();
                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                pushpin.Name = report.disaster;
                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                myMap.Children.Add(pushpin);
            }
#endif
        }
        private async void loadRF()
        {
#if WINDOWS_APP
            var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
            foreach (Risk_Factor_Report report in reports)
            {
                Pushpin pushpin = new Pushpin();
                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                pushpin.Name = report.risk_factor;
                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                myMap.Children.Add(pushpin);
            }
#endif
        }
        private async void loadDisease()
        {


        }



        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
#if WINDOWS_APP
            MessageDialog dialog = new MessageDialog(((Pushpin)sender).Name);
            await dialog.ShowAsync();
#endif
        }
    }
}
