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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;


namespace DEDI
{
    public sealed partial class HomePage
    {
        private Health_Worker user;
#if WINDOWS_APP
        Map myMap;
#endif
        string user_postcode;
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
            var diseaseReports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
            List<Report> all = new List<Report>();
            ImageSource src = null;
            foreach (Disaster_Report disasteritem in disasterReports)
            {
                if (disasteritem.disaster.Equals("Earthquake")) src = new BitmapImage(new Uri("ms-appx:/Assets/earthquake_btn.png"));
                else if (disasteritem.disaster.Equals("Flood")) src = new BitmapImage(new Uri("ms-appx:/Assets/flood_btn.png"));
                else if (disasteritem.disaster.Equals("Storm")) src = new BitmapImage(new Uri("ms-appx:/Assets/storm_btn.png"));
                else if (disasteritem.disaster.Equals("Wildfire")) src = new BitmapImage(new Uri("ms-appx:/Assets/wildfire_btn.png"));
                else if (disasteritem.disaster.Equals("Tsunami")) src = new BitmapImage(new Uri("ms-appx:/Assets/tsunami_btn.png"));
                else if (disasteritem.disaster.Equals("Volcanic eruption")) src = new BitmapImage(new Uri("ms-appx:/Assets/volcanic_btn.png"));
                all.Add(new Report
                {
                    name = disasteritem.disaster,
                    hw_id = disasteritem.hw_id,
                    latitude = disasteritem.latitude,
                    longitude = disasteritem.longitude,
                    description = disasteritem.description,
                    reported_time = disasteritem.reported_time,
                    ocurred_time = disasteritem.ocurred_time,
                    icon = src
                });
            }
            foreach (Risk_Factor_Report riskitem in riskReports)
            {
                if (riskitem.risk_factor.Equals("Contaminated food")) src = new BitmapImage(new Uri("ms-appx:/Assets/food_btn.png"));
                else if (riskitem.risk_factor.Equals("Contaminated water")) src = new BitmapImage(new Uri("ms-appx:/Assets/water_btn.png"));
                else if (riskitem.risk_factor.Equals("Crowding")) src = new BitmapImage(new Uri("ms-appx:/Assets/crowding_btn.png"));
                else if (riskitem.risk_factor.Equals("Poor sanitation")) src = new BitmapImage(new Uri("ms-appx:/Assets/sanitation_btn.png"));
                all.Add(new Report
                {
                    name = riskitem.risk_factor,
                    hw_id = riskitem.hw_id,
                    latitude = riskitem.latitude,
                    longitude = riskitem.longitude,
                    description = riskitem.description,
                    reported_time = riskitem.reported_time,
                    ocurred_time = riskitem.ocurred_time,
                    icon = src
                });
            }
            foreach (Disease_Report diseaseitem in diseaseReports)
            {
                src = new BitmapImage(new Uri("ms-appx:/Assets/disease_report_btn.png"));
                all.Add(new Report
                {
                    name = "Disease report",
                    hw_id = diseaseitem.hw_id,
                    latitude = diseaseitem.latitude,
                    longitude = diseaseitem.longitude,
                    description = diseaseitem.description,
                    reported_time = diseaseitem.reported_time,
                    ocurred_time = diseaseitem.ocurred_time,
                    icon = src
                });
            }
            all = all.OrderByDescending(o => o.ocurred_time).ToList();
            lv.ItemsSource = all;
            lv.SelectionMode = ListViewSelectionMode.None;
        }
        private async void loadNoti()
        {


            
           
            

            StackPanel allnoti = FindChildControl<StackPanel>(NotiSection, "AllNotiStack") as StackPanel;
            List<Message> msg = await App.MobileService.GetTable<Message>().Where(r => r.hw_receiver_id == user.id).ToListAsync();
            foreach (Message m in msg)
            {
                ImageBrush myBrush = new ImageBrush();
                Image image = new Image();
                if (m.status == "important") image.Source = new BitmapImage(new Uri("ms-appx:/Assets/noti_tab_yellow.png"));
                else if (m.status == "very important") image.Source = new BitmapImage(new Uri("ms-appx:/Assets/noti_tab_red.png"));
                else image.Source = new BitmapImage(new Uri("ms-appx:/Assets/noti_tab_green.png"));
                myBrush.ImageSource = image.Source;
                List<Health_Worker> hw = await App.MobileService.GetTable<Health_Worker>().Where(h => h.id == m.hw_sender_id).ToListAsync();
                TextBlock name = new TextBlock();
                name.Text = hw[0].fname + " " + hw[0].lname;
                TextBlock sent_time = new TextBlock();
                sent_time.Text = m.sent_time + "";
                TextBlock topic = new TextBlock();
                topic.Text = m.topic;
                Health_Worker sender = hw[0];
                Grid item = new Grid()
                {
                    Width = 360,
                    Margin = new Windows.UI.Xaml.Thickness(10),
                    Height = 86
                };
                item.Background = myBrush;
                item.Children.Add(topic);
                item.Children.Add(name);
                item.Children.Add(sent_time);
                item.HorizontalAlignment = HorizontalAlignment.Left;
                item.VerticalAlignment = VerticalAlignment.Top;

                // Define the Columns
                ColumnDefinition colDef1 = new ColumnDefinition();
                ColumnDefinition colDef2 = new ColumnDefinition();
                ColumnDefinition colDef3 = new ColumnDefinition();
                item.ColumnDefinitions.Add(colDef1);
                item.ColumnDefinitions.Add(colDef2);
                item.ColumnDefinitions.Add(colDef3);

                // Define the Rows
                RowDefinition rowDef1 = new RowDefinition();
                RowDefinition rowDef2 = new RowDefinition();
                RowDefinition rowDef3 = new RowDefinition();
                RowDefinition rowDef4 = new RowDefinition();
                RowDefinition rowDef5 = new RowDefinition();
                item.RowDefinitions.Add(rowDef1);
                item.RowDefinitions.Add(rowDef2);
                item.RowDefinitions.Add(rowDef3);
                item.RowDefinitions.Add(rowDef4);
                item.RowDefinitions.Add(rowDef5);
                Grid.SetRow(topic, 1);
                Grid.SetColumn(topic, 1);
                Grid.SetRow(name, 2);
                Grid.SetColumn(name, 1);
                Grid.SetRow(sent_time, 3);
                Grid.SetColumn(sent_time, 1);
                allnoti.Children.Insert(0,item);
            }

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
                TextBlock positionTbl = FindChildControl<TextBlock>(ProfileSection, "positionTbl") as TextBlock;
                positionTbl.Text = user.position;

                var following = await App.MobileService.GetTable<Follow>().Where(p => p.follower_hw_id == user.id).ToListAsync();
                TextBlock followingTbl = FindChildControl<TextBlock>(ProfileSection, "followingTbl") as TextBlock;
                followingTbl.Text = following.Count + "";

                var follower = await App.MobileService.GetTable<Follow>().Where(p => p.following_hw_id == user.id).ToListAsync();
                TextBlock followerTbl = FindChildControl<TextBlock>(ProfileSection, "followersTbl") as TextBlock;
                followerTbl.Text = follower.Count + "";

                var disaster_rp = await App.MobileService.GetTable<Disaster_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                var disease_rp = await App.MobileService.GetTable<Disease_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                var rf_rp = await App.MobileService.GetTable<Risk_Factor_Report>().Where(p => p.hw_id == user.id).ToListAsync();
                TextBlock myreport = FindChildControl<TextBlock>(ProfileSection, "myreportsTbl") as TextBlock;
                int no_myreport = disaster_rp.Count + disease_rp.Count + rf_rp.Count;
                myreport.Text = no_myreport + "";

#if WINDOWS_APP
                myMap = FindChildControl<Map>(MapSection, "myMap") as Map;
                myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                myMap.ZoomLevel = 10;
                myMap.MapType = MapType.Road;
                myMap.Width = 420;
                myMap.Height = 480;
                myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
                {
                    Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    response = await client.GetAsync(Uri);
                    result = await response.Content.ReadAsStringAsync();
                    ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    serializer = new DataContractJsonSerializer(typeof(RootObject));
                    list = serializer.ReadObject(ms);
                    jsonResponse = list as RootObject;
                    user_postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                }
                loadRF();
                loadDisaster();
                loadReports();
                loadDisease();
                loadNoti();
#endif
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

      public static T DeserializeJSon<T>(string jsonString)
      {
          DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
          MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
          T obj = (T)ser.ReadObject(stream);
          return obj;
      }
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
        #if WINDOWS_APP
        private async void loadDisaster()
        {

            var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
            if(user.position =="Village Health Volunteer"||user.position=="Tambon Health Promoting Hospital Officer"||user.position=="District Public Health Officer")
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
                 
                        if(postcode==user_postcode){
                    Pushpin pushpin = new Pushpin();
                    pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                            pushpin.Name = report.id;

                            pushpin.Background = new SolidColorBrush(Colors.GreenYellow); 
                    MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                    myMap.Children.Add(pushpin);
                    }
                }
        
            }
            else {
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
            if(user.position =="Village Health Volunteer"||user.position=="Tambon Health Promoting Hospital Officer"||user.position=="District Public Health Officer"){
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
                        if(postcode==user_postcode){
                            Pushpin pushpin = new Pushpin();
                            pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                            pushpin.Name = report.id;

                            pushpin.Background = new SolidColorBrush(Colors.DeepPink); 
                            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                            myMap.Children.Add(pushpin);
                        }
                }

            }
            else{
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
            var d = await App.MobileService.GetTable<Disease_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
            if (d.Count > 0)
            {
                string Title = "Probability of this disease report";
                string Content = "Cholera: " + Math.Round((d[0].cholera * 100), 2) + " %" + "\n" + "Shigella: " + Math.Round((d[0].shigella * 100), 2) + " %" + "\n" + "Salmoella: " + Math.Round((d[0].salmonella * 100), 2) + " %" + "\n" + "Rotavirus: " + Math.Round((d[0].rotavirus * 100), 2) + " %" + "\n" + "Others: " + Math.Round((d[0].others * 100), 2) + " %" + "\n\n" + d[0].ocurred_time.Date;
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
        private void MyReport_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyReportPage), user);

        }
        private void Following_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FollowingPage), user);
        }

        private void Follower_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FollowerPage), user);
        }
    }
}
