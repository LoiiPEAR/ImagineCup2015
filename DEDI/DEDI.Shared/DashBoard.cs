
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

namespace DEDI
{
    public sealed partial class DashBoard
    {
#if WINDOWS_APP
        Map myMap;
#endif
        public class NumberOfCases
        {
            public string date { get; set; }
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
                var disease_report = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
                TextBlock no_case = FindChildControl<TextBlock>(PredictionSection, "NoOfCasesTbl") as TextBlock;
                no_case.Text = disease_report.Count+"";
                foreach(Disease_Report report in disease_report){
                    Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    response = await client.GetAsync(Uri);
                    result = await response.Content.ReadAsStringAsync();
                    ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    list = serializer.ReadObject(ms);
                    jsonResponse = list as RootObject;
                    string report_postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                    if (user_postcode.Equals(report_postcode))
                    {
                        var patient = await App.MobileService.GetTable<Patient>().Where(p => p.id==report.patient_id).ToListAsync();
                        if (patient.Count > 0)
                        {
                            if (patient[0].gender == "F") female++;
                            else male++;
                            if (CalculateAge(patient[0].dob) <= 15) child++;
                        }
                        var prob = await App.MobileService.GetTable<Bayesian_Prob>().Where(p => p.id == report.prob_id).ToListAsync();

                    }
                    
                    
                   
                }
                TextBlock no_child = FindChildControl<TextBlock>(PredictionSection, "NoOfChildTbl") as TextBlock;
                no_child.Text = child + "";
                TextBlock no_male = FindChildControl<TextBlock>(PredictionSection, "NoOfMaleTbl") as TextBlock;
                no_male.Text = male + "";
                TextBlock no_female = FindChildControl<TextBlock>(PredictionSection, "NoOfFemaleTbl") as TextBlock;
                no_female.Text = female + "";

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
#endif
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void loadgraph()
        {
            Random rand = new Random();
            List<NumberOfCases> All = new List<NumberOfCases>();
            List<NumberOfCases> Cholera = new List<NumberOfCases>();
            List<NumberOfCases> Rotavirus = new List<NumberOfCases>();
            List<NumberOfCases> Shigella = new List<NumberOfCases>();
            List<NumberOfCases> Typhoid = new List<NumberOfCases>();
            List<NumberOfCases> Others = new List<NumberOfCases>();
            All.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Chart NoOfCasesChart = FindChildControl<Chart>(PredictionSection, "NoOfCasesChart") as Chart;
            (NoOfCasesChart.Series[0] as ColumnSeries).ItemsSource = All;
            (NoOfCasesChart.Series[1] as ColumnSeries).ItemsSource = Cholera;
            (NoOfCasesChart.Series[2] as ColumnSeries).ItemsSource = Rotavirus;
            (NoOfCasesChart.Series[3] as ColumnSeries).ItemsSource = Shigella;
            (NoOfCasesChart.Series[4] as ColumnSeries).ItemsSource = Typhoid;
            (NoOfCasesChart.Series[5] as ColumnSeries).ItemsSource = Others;
            Button NoOfCasesBtn = FindChildControl<Button>(PredictionSection, "NoOfCasesBtn") as Button;
            NoOfCasesBtn.Visibility = Visibility.Collapsed;
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

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid ProbReport = FindChildControl<Grid>(PredictionSection, "ProbReport") as Grid;
            ProbReport.Height = 250;
            Button MoreBtn = FindChildControl<Button>(PredictionSection, "MoreBtn") as Button;
            MoreBtn.Visibility = Visibility.Collapsed;
            Button LessBtn = FindChildControl<Button>(PredictionSection, "LessBtn") as Button;
            LessBtn.Visibility = Visibility.Visible;
        }

        private void LessBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid ProbReport = FindChildControl<Grid>(PredictionSection, "ProbReport") as Grid;
            ProbReport.Height = 130;
            Button MoreBtn = FindChildControl<Button>(PredictionSection, "MoreBtn") as Button;
            MoreBtn.Visibility = Visibility.Visible;
            Button LessBtn = FindChildControl<Button>(PredictionSection, "LessBtn") as Button;
            LessBtn.Visibility = Visibility.Collapsed;
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
               // string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
                string Title= "Disease Report";
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



       
    }
}
