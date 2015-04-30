
#if WINDOWS_APP
using Bing.Maps;
#endif
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI;
using System.Linq;
using Windows.UI.Xaml.Shapes;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls.Maps;
#endif

namespace DEDI
{
    public sealed partial class DashBoard
    {
        double cholera = 0;
        double shigella = 0;
        double rotavirus = 0;
        double salmonella = 0;
#if WINDOWS_APP
        Map myMap;
#endif
        public class NumberOfCases
        {
            public DateTime date { get; set; }
            public int cases { get; set; }
        }
        Health_Worker user;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            loaddata();
        }
        string user_postcode;
        private async void loaddata()
        {
            try
            {
                var client = new HttpClient();
                Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                RootObject jsonResponse = list as RootObject;
                user_postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;

                int male = 0;
                int female = 0;
                int child = 0;

                List<Dashboard_Report> dashboard_report = new List<Dashboard_Report>();
               
                var disease_report = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
#if WINDOWS_APP
                TextBlock NoOfCasesTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfCasesTbl") as TextBlock;
#endif
                NoOfCasesTbl.Text = disease_report.Count + "";
                foreach(Disease_Report report in disease_report){
                    Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    response = await client.GetAsync(Uri);
                    result = await response.Content.ReadAsStringAsync();
                    ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    list = serializer.ReadObject(ms);
                    jsonResponse = list as RootObject;
                    string report_postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                   
                        var patient = await App.MobileService.GetTable<Patient>().Where(p => p.id==report.patient_id).ToListAsync();
                        if (patient.Count > 0)
                        {
                            if (patient[0].gender == "F" && CalculateAge(patient[0].dob) > 12) female++;
                            else if (patient[0].gender == "M" && CalculateAge(patient[0].dob) > 12) male++;
                            else if (CalculateAge(patient[0].dob) <= 12) child++;
                }
                        List<Bayesian_Prob> prob = await App.MobileService.GetTable<Bayesian_Prob>().Where(p => p.id == report.prob_id).ToListAsync();
                        cholera += prob[0].Cholera;
                        shigella += prob[0].Shigella;
                        rotavirus += prob[0].Rotavirus;
                        salmonella += prob[0].Samonella;
                        
                        dashboard_report.Add(new Dashboard_Report() { 
                            id ="ReportID: "+report.id.Substring(0,10),
                            CholeraPercent = Math.Round((prob[0].Cholera*100),2)+"%",
                            CholeraWidth = prob[0].Cholera*200,
                            ShigellaPercent = Math.Round((prob[0].Shigella*100),2)+"%",
                            ShigellaWidth = prob[0].Shigella*200,
                            SalmonellaPercent = Math.Round((prob[0].Samonella * 100), 2) + "%",
                            SalmonellaWidth = prob[0].Samonella * 200,
                            RotavirusPercent = Math.Round((prob[0].Rotavirus * 100), 2) + "%",
                            RotavirusWidth = prob[0].Rotavirus * 200

                        });
                   
                    
                   
                   
                }
#if WINDOWS_APP
                TextBlock NoOfChildTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfChildTbl") as TextBlock;
#endif
                NoOfChildTbl.Text = child + "";
#if WINDOWS_APP
                TextBlock NoOfMaleTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfMaleTbl") as TextBlock;
#endif
                NoOfMaleTbl.Text = male + "";
#if WINDOWS_APP
                TextBlock NoOfFemaleTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfFemaleTbl") as TextBlock;
#endif
                NoOfFemaleTbl.Text = female + "";

#if WINDOWS_APP
                TextBlock NoCholeraTbl = FindChildControl<TextBlock>(PredictionSection, "NoCholeraTbl") as TextBlock;
#endif
                NoCholeraTbl.Text = Math.Floor(cholera) + "";
#if WINDOWS_APP
                TextBlock NoShigellaTbl = FindChildControl<TextBlock>(PredictionSection, "NoShigellaTbl") as TextBlock;
#endif
                NoShigellaTbl.Text = Math.Floor(shigella) + "";
#if WINDOWS_APP
                TextBlock NoRotaTbl = FindChildControl<TextBlock>(PredictionSection, "NoRotaTbl") as TextBlock;
#endif
                NoRotaTbl.Text = Math.Floor(rotavirus) + "";
#if WINDOWS_APP
                TextBlock NoSalmonellaTbl = FindChildControl<TextBlock>(PredictionSection, "NoSalmonellaTbl") as TextBlock;
#endif
                NoSalmonellaTbl.Text = Convert.ToInt32(salmonella) + "";
#if WINDOWS_APP
                ListView ReportLv = FindChildControl<ListView>(PredictionSection, "ReportLv") as ListView;
#endif
                ReportLv.ItemsSource = dashboard_report;

                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                               TimeSpan.FromSeconds(10));
#if WINDOWS_APP
                myMap = FindChildControl<Map>(ResponsibleAreaSection, "myMap") as Map;
                myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                myMap.ZoomLevel = 17;
                myMap.MapType = MapType.Road;
                myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                loadgraph();
                loadDisaster();
                loadRF();
                loadDisease();
#endif
#if WINDOWS_PHONE_APP
                InitializeMap();
#endif     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void loadgraph()
        {
            try
            {
                Random rand = new Random();
                //List<NumberOfCases> All = new List<NumberOfCases>();
                List<NumberOfCases> Cholera = new List<NumberOfCases>();
                List<NumberOfCases> Rotavirus = new List<NumberOfCases>();
                List<NumberOfCases> Shigella = new List<NumberOfCases>();
                List<NumberOfCases> Salmonella = new List<NumberOfCases>();
                //List<NumberOfCases> Others = new List<NumberOfCases>();

                var disease_report = await App.MobileService.GetTable<Disease_Report>().ToListAsync();

                //var h = disease_report.GroupBy(d => d.ocurred_time.Date ).Select(d => new{dateOccurred=d.Key,noOfCases=d.Count()}).OrderBy(t => t.dateOccurred);
                var all = disease_report.GroupBy(d => d.ocurred_time.Date).Select(d => new { dateOccurred = d.Key, noOfCases = d.Count() }).OrderBy(t => t.dateOccurred);
                //var others = disease_report.Where(d => d.others > 0.5).GroupBy(d => d.ocurred_time.Date).Select(d => new { dateOccurred = d.Key, noOfCases = d.Count() }).OrderBy(t => t.dateOccurred);

                //foreach(var report in h)
                //{
                //    All.Add(new NumberOfCases() {date = report.dateOccurred , cases = report.noOfCases});
                //}

                foreach (var report in all)
                {
                    Cholera.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)cholera });
                    Rotavirus.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)rotavirus });
                    Shigella.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)shigella });
                    Salmonella.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)salmonella });
                }
                //foreach (var report in others)
                //{
                //    Others.Add(new NumberOfCases() { date = report.dateOccurred, cases = report.noOfCases });
                //}
#if WINDOWS_APP
                Chart NoOfCasesChart = FindChildControl<Chart>(PredictionSection, "NoOfCasesChart") as Chart;
#endif
                //(NoOfCasesChart.Series[0] as ColumnSeries).ItemsSource = All;
                (NoOfCasesChart.Series[0] as ColumnSeries).ItemsSource = Cholera;
                (NoOfCasesChart.Series[1] as ColumnSeries).ItemsSource = Rotavirus;
                (NoOfCasesChart.Series[2] as ColumnSeries).ItemsSource = Shigella;
                (NoOfCasesChart.Series[3] as ColumnSeries).ItemsSource = Salmonella;

                (NoOfCasesChart.Series[0] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[1] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[2] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[3] as ColumnSeries).IsEnabled = false;
                //(NoOfCasesChart.Series[5] as ColumnSeries).ItemsSource = Others;
#if WINDOWS_APP
                Button NoOfCasesBtn = FindChildControl<Button>(PredictionSection, "NoOfCasesBtn") as Button;
#endif
                NoOfCasesBtn.Visibility = Visibility.Collapsed;
            }
            catch (Exception e) { }
        }
        public int CalculateAge(DateTime birthDate)
        {
            var now = DateTime.Today;
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }
        private void NoOfCasesBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
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
#if WINDOWS_APP
        private async void loadDisaster()
        {
            try
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
            catch (Exception e) { }

        }
        private async void loadRF()
        {
            try
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
            catch (Exception e) { }
        }

        private async void loadDisease()
        {
            try
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
            catch (Exception e) { }

        }



        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            try
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
                    // string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
                    string Title = "Disease Report";
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
            catch (Exception ex) { }

        }
#endif
#if WINDOWS_PHONE_APP
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
            try
            {
                var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
                //if (user.position == "Village Health Volunteer" || user.position == "Tambon Health Promoting Hospital Officer" || user.position == "District Public Health Officer")
                //{
                //    var client = new HttpClient();
                foreach (Disease_Report report in reports)
                {
                    //        var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + user.latitude + "," + user.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    //        var response = await client.GetAsync(Uri);
                    //        var result = await response.Content.ReadAsStringAsync();
                    //        var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    //        var serializer = new DataContractJsonSerializer(typeof(RootObject));
                    //        var list = serializer.ReadObject(ms);
                    //        var jsonResponse = list as RootObject;
                    //        string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                    //        //if (postcode == user_postcode)
                    //        //{
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "p");

                    //        //}
                }
                //}
            }
            catch (Exception e) { }
        }

        private async void loadDisaster()
        {
            try
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
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "g");

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
            catch (Exception e) { }

        }

        private async void loadRF()
        {
            try
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
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "b");

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
            catch (Exception e) { }
        }
        public void AddPushpin(BasicGeoposition location, string text, string color)
        {

            try
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
            catch (Exception e) { }
        }

        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                MessageDialog dialog;
                var disaster_reports = await App.MobileService.GetTable<Disaster_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
                if (disaster_reports.Count > 0)
                {
                    string Title = disaster_reports[0].disaster;
                    string Content = disaster_reports[0].description + "\n" + disaster_reports[0].ocurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
                var d = await App.MobileService.GetTable<Disease_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
                if (d.Count > 0)
                {
                    //string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
                    string Title = "Probability of this disease report";
                    string Content = "Cholera: " + d[0].cholera + "\n" + "Shigella: " + d[0].shigella + "\n" + "Salmoella: " + d[0].salmonella + "\n" + "Rotavirus: " + d[0].rotavirus + "\n" + "Others: " + d[0].others + "\n\n" + d[0].ocurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
                var rf_reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.id == ((Grid)sender).Name).ToListAsync();
                if (rf_reports.Count > 0)
                {
                    string Title = rf_reports[0].risk_factor;
                    string Content = rf_reports[0].description + "\n" + rf_reports[0].ocurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
            }
            catch (Exception e) { }

        }
#endif
        private void TreatmentSuggestion_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TreatmentSuggestionPage), user);
        }


       
    }
    class Dashboard_Report
    {
        public string id { get; set; }
        public double CholeraWidth { get; set; }
        public double ShigellaWidth { get; set; }
        public double RotavirusWidth { get; set; }
        public double SalmonellaWidth { get; set; }
        public string CholeraPercent { get; set; }
        public string ShigellaPercent { get; set; }
        public string RotavirusPercent { get; set; }
        public string SalmonellaPercent { get; set; }
    }
}
