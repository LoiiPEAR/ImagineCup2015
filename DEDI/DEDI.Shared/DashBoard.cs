
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
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
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
        double others = 0;
        int check_cholera = 0;
        int check_shigella = 0;
        int check_rotavirus = 0;
        int check_salmonella = 0;
        int check_other = 0;
        int check_all = 0;

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

                
                var today = DateTime.Today;
                var month = new DateTime(today.Year, today.Month - 1, 1);
#if WINDOWS_APP
                DatePicker StartDatePicker = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
#endif
                StartDatePicker.Date = month;

                

                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                               TimeSpan.FromSeconds(10));
#if WINDOWS_APP
                myMap = FindChildControl<Map>(ResponsibleAreaSection, "myMap") as Map;
                myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                myMap.ZoomLevel = 10;
                myMap.MapType = MapType.Road;
                myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                loadgraph();
                ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
                ToggleButton CholeraBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "CholeraBtn") as ToggleButton;
                ToggleButton RotavirusBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "RotavirusBtn") as ToggleButton;
                ToggleButton SalmonellaBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "SalmonellaBtn") as ToggleButton;
                ToggleButton ShigellaBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "ShigellaBtn") as ToggleButton;
                ToggleButton OtherBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "OtherBtn") as ToggleButton;
                AllBtn.IsEnabled = false;
                CholeraBtn.IsChecked = false;
                RotavirusBtn.IsChecked = false;
                SalmonellaBtn.IsChecked = false;
                ShigellaBtn.IsChecked = false;
                OtherBtn.IsChecked = false;
                check_all = 1;
                loadDisease();
                loadDisaster();
                loadRF();
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
                List<NumberOfCases> Others = new List<NumberOfCases>();

                var disease_report = await App.MobileService.GetTable<Disease_Report>().ToListAsync();

                //var h = disease_report.GroupBy(d => d.occurred_time.Date ).Select(d => new{dateOccurred=d.Key,noOfCases=d.Count()}).OrderBy(t => t.dateOccurred);
                var all = disease_report.GroupBy(d => d.occurred_time.Date).Select(d => new { dateOccurred = d.Key, noOfCases = d.Count() }).OrderBy(t => t.dateOccurred);
                //var others = disease_report.Where(d => d.others > 0.5).GroupBy(d => d.occurred_time.Date).Select(d => new { dateOccurred = d.Key, noOfCases = d.Count() }).OrderBy(t => t.dateOccurred);

                //foreach(var report in h)
                //{
                //    All.Add(new NumberOfCases() {date = report.dateOccurred , cases = report.noOfCases});
                //}

                foreach (var report in all)
                {
                    DatePicker startdate = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
                    DatePicker enddate = FindChildControl<DatePicker>(ResponsibleAreaSection, "EndDatePicker") as DatePicker;
                    if (report.dateOccurred < enddate.Date && report.dateOccurred > startdate.Date)
                    {
                        Cholera.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)Math.Round(cholera) });
                        Rotavirus.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)Math.Round(rotavirus) });
                        Shigella.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)Math.Round(shigella) });
                        Salmonella.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)Math.Round(salmonella) });
                        Others.Add(new NumberOfCases() { date = report.dateOccurred, cases = (int)(disease_report.Count - Math.Round(cholera) - Math.Round(shigella) - Math.Round(rotavirus) - Math.Round(salmonella)) });
                    }
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
                (NoOfCasesChart.Series[4] as ColumnSeries).ItemsSource = Others;

                (NoOfCasesChart.Series[0] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[1] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[2] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[3] as ColumnSeries).IsEnabled = false;
                (NoOfCasesChart.Series[4] as ColumnSeries).IsEnabled = false;
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
        private async void loadNum()
        {
            try
            {
                int male = 0;
                int female = 0;
                int child = 0;
                int NoofCases = 0;
                cholera = 0;
                shigella = 0;
                rotavirus = 0;
                salmonella = 0;
                others = 0;
                DatePicker startdate = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
                DatePicker enddate = FindChildControl<DatePicker>(ResponsibleAreaSection, "EndDatePicker") as DatePicker;
                var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
               
                List<Dashboard_Report> dashboard_report = new List<Dashboard_Report>();
                foreach (Disease_Report report in reports)
                {

                    if (report.occurred_time < enddate.Date && report.occurred_time.Date > startdate.Date)
                    {
                        var patient = await App.MobileService.GetTable<Patient>().Where(p => p.id == report.patient_id).ToListAsync();
                        if (patient.Count > 0)
                        {
                            if (patient[0].gender == "F" && CalculateAge(patient[0].dob) > 12) female++;
                            else if (patient[0].gender == "M" && CalculateAge(patient[0].dob) > 12) male++;
                            else if (CalculateAge(patient[0].dob) <= 12) child++;
                        }
                        cholera += report.cholera;
                        shigella += report.shigella;
                        salmonella += report.salmonella;
                        rotavirus += report.rotavirus;
                        NoofCases++;
                        dashboard_report.Add(new Dashboard_Report()
                        {
                            id = "ReportID: " + report.id.Substring(0, 10),
                            CholeraPercent = Math.Round((report.cholera * 100), 2) + "%",
                            CholeraWidth = report.cholera * 200,
                            ShigellaPercent = Math.Round((report.shigella * 100), 2) + "%",
                            ShigellaWidth = report.shigella * 200,
                            SalmonellaPercent = Math.Round((report.salmonella * 100), 2) + "%",
                            SalmonellaWidth = report.salmonella * 200,
                            RotavirusPercent = Math.Round((report.rotavirus * 100), 2) + "%",
                            RotavirusWidth = report.rotavirus * 200

                        });
                    }
                        
                    
                }

                Image lt5 = FindChildControl<Image>(PredictionSection, "lt5") as Image;
                Image lt10 = FindChildControl<Image>(PredictionSection, "lt10") as Image;
                Image mt10 = FindChildControl<Image>(PredictionSection, "mt10") as Image;

                if (NoofCases < 5)
                {
                    lt5.Visibility = Visibility.Visible;
                    lt10.Visibility = Visibility.Collapsed;
                    mt10.Visibility = Visibility.Collapsed;
                }
                else if (NoofCases < 10 && reports.Count >= 5)
                {
                    lt5.Visibility = Visibility.Collapsed;
                    lt10.Visibility = Visibility.Visible;
                    mt10.Visibility = Visibility.Collapsed;
                }
                else if (NoofCases >= 10)
                {
                    lt5.Visibility = Visibility.Collapsed;
                    lt10.Visibility = Visibility.Collapsed;
                    mt10.Visibility = Visibility.Visible;
                }
                TextBlock NoOfCasesTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfCasesTbl") as TextBlock;
                NoOfCasesTbl.Text = NoofCases + "";

                TextBlock NoOfChildTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfChildTbl") as TextBlock;
                NoOfChildTbl.Text = child + "";

                TextBlock NoOfMaleTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfMaleTbl") as TextBlock;
                NoOfMaleTbl.Text = male + "";

                TextBlock NoOfFemaleTbl = FindChildControl<TextBlock>(PredictionSection, "NoOfFemaleTbl") as TextBlock;
                NoOfFemaleTbl.Text = female + "";

                TextBlock NoCholeraTbl = FindChildControl<TextBlock>(PredictionSection, "NoCholeraTbl") as TextBlock;

                NoCholeraTbl.Text = Math.Round(cholera) + "";

                TextBlock NoShigellaTbl = FindChildControl<TextBlock>(PredictionSection, "NoShigellaTbl") as TextBlock;

                NoShigellaTbl.Text = Math.Round(shigella) + "";

                TextBlock NoRotaTbl = FindChildControl<TextBlock>(PredictionSection, "NoRotaTbl") as TextBlock;

                NoRotaTbl.Text = Math.Round(rotavirus) + "";

                TextBlock NoSalmonellaTbl = FindChildControl<TextBlock>(PredictionSection, "NoSalmonellaTbl") as TextBlock;

                NoSalmonellaTbl.Text = Math.Round(salmonella) + "";

                TextBlock NoOthersTbl = FindChildControl<TextBlock>(PredictionSection, "NoOthersTbl") as TextBlock;
                NoOthersTbl.Text = NoofCases - Math.Round(cholera) - Math.Round(shigella) - Math.Round(rotavirus) - Math.Round(salmonella) + "";

                ListView ReportLv = FindChildControl<ListView>(PredictionSection, "ReportLv") as ListView;

                ReportLv.ItemsSource = dashboard_report;
            }
            catch (Exception e) { }

        }
        private async void loadDisaster()
        {
            try
            {
                DatePicker startdate = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
                DatePicker enddate = FindChildControl<DatePicker>(ResponsibleAreaSection, "EndDatePicker") as DatePicker;
                        
                var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
               
                    var client = new HttpClient();
                    foreach (Disaster_Report report in reports)
                    {
                        //var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                        //var response = await client.GetAsync(Uri);
                        //var result = await response.Content.ReadAsStringAsync();
                        //var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                        //var serializer = new DataContractJsonSerializer(typeof(RootObject));
                        //var list = serializer.ReadObject(ms);
                        //var jsonResponse = list as RootObject;
                        //string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;

                        //if (postcode == user_postcode)
                        //{
                       if (report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                        {
                            Pushpin pushpin = new Pushpin();
                            pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                            pushpin.Name = report.id;

                            pushpin.Background = new SolidColorBrush(Colors.GreenYellow);
                            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                            myMap.Children.Add(pushpin);
                        }
                        //}
                        }


                    }
            catch (Exception e) { }

        }
        private async void loadRF()
        {
            try
            {
                DatePicker startdate = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
                DatePicker enddate = FindChildControl<DatePicker>(ResponsibleAreaSection, "EndDatePicker") as DatePicker;

                var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
                
                    var client = new HttpClient();
                    foreach (Risk_Factor_Report report in reports)
                    {
                        //var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                        //var response = await client.GetAsync(Uri);
                        //var result = await response.Content.ReadAsStringAsync();
                        //var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                        //var serializer = new DataContractJsonSerializer(typeof(RootObject));
                        //var list = serializer.ReadObject(ms);
                        //var jsonResponse = list as RootObject;
                        //string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                        //if (postcode == user_postcode)
                        //{
                        if (report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                        {
                            Pushpin pushpin = new Pushpin();
                            pushpin.Tapped += new TappedEventHandler(pushpinTapped);

                            pushpin.Background = new SolidColorBrush(Colors.Blue);
                            pushpin.Name = report.id;
                            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                            myMap.Children.Add(pushpin);
                        }
                        //}
                    }

                

            }
            catch (Exception e) { }
        }

        private async void loadDisease()
        {
            try
            {
                myMap.Children.Clear();

                var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
                Slider Prob = FindChildControl<Slider>(ResponsibleAreaSection, "ProbabilitySider") as Slider;
            
                    var client = new HttpClient();
                    foreach (Disease_Report report in reports)
                    {
                        //var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                        //var response = await client.GetAsync(Uri);
                        //var result = await response.Content.ReadAsStringAsync();
                        //var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                        //var serializer = new DataContractJsonSerializer(typeof(RootObject));
                        //var list = serializer.ReadObject(ms);
                        //var jsonResponse = list as RootObject;
                        //string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                        //if (postcode == user_postcode)
                        //{
                        DatePicker startdate = FindChildControl<DatePicker>(ResponsibleAreaSection, "StartDatePicker") as DatePicker;
                        DatePicker enddate = FindChildControl<DatePicker>(ResponsibleAreaSection, "EndDatePicker") as DatePicker;
                
                            if(((check_cholera ==1||check_all==1) && report.cholera>Prob.Value/100)&&
                                report.occurred_time<enddate.Date&&report.occurred_time>startdate.Date){
                            Pushpin pushpin = new Pushpin();
                            pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                            pushpin.Name = report.id;

                            pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFE, 0xC6, 0x44));
                            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                            myMap.Children.Add(pushpin);
                        }
                            else if (((check_rotavirus == 1 || check_all == 1) && report.rotavirus > Prob.Value / 100) &&
                            report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                            {
                                Pushpin pushpin = new Pushpin();
                                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                                pushpin.Name = report.id;

                                pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xA9, 0xAB, 0xAE));
                                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                                myMap.Children.Add(pushpin);
                            }
                            else if (((check_salmonella == 1 || check_all == 1) && report.salmonella > Prob.Value / 100) &&
                        report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                            {
                                Pushpin pushpin = new Pushpin();
                                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                                pushpin.Name = report.id;

                                pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xAE, 0xC8));
                                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                                myMap.Children.Add(pushpin);
                            }
                            else if (((check_shigella == 1 || check_all == 1) && report.shigella > Prob.Value / 100) &&
                    report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                            {
                                Pushpin pushpin = new Pushpin();
                                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                                pushpin.Name = report.id;

                                pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x6D, 0xC3, 0x9F));
                                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                                myMap.Children.Add(pushpin);
                            }
                            else if (((check_other == 1 || check_all == 1) && report.others > Prob.Value / 100) &&
                report.occurred_time < enddate.Date && report.occurred_time > startdate.Date)
                            {
                                Pushpin pushpin = new Pushpin();
                                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                                pushpin.Name = report.id;
                                
                                pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF,0xA9,0xAB,0xAE));
                                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                                myMap.Children.Add(pushpin);
                            }
                            else
                            {
                                Pushpin pushpin = new Pushpin();
                                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                                pushpin.Name = report.id;

                                pushpin.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xA9, 0xAB, 0xAE));
                                MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
                                myMap.Children.Add(pushpin);
                            }

                        //}
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
                    string Content = disaster_reports[0].description + "\n" + disaster_reports[0].occurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
                var disease_reports = await App.MobileService.GetTable<Disease_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
                if (disease_reports.Count > 0)
                {
                    // string Title = "Chance of cholera:" + disease_reports[0].cholera + "\nChance of shigella:" + disease_reports[0].shigella + "\nChance of salmonella:" + disease_reports[0].simolnelle + "\nChance of rotavirus:" + disease_reports[0].rotavirus + "\nChance of others:" + disease_reports[0].others;
                    string Title = "Disease Report";
                    string Content = disease_reports[0].description + "\n" + disease_reports[0].occurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
                var rf_reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.id == ((Pushpin)sender).Name).ToListAsync();
                if (rf_reports.Count > 0)
                {
                    string Title = rf_reports[0].risk_factor;
                    string Content = rf_reports[0].description + "\n" + rf_reports[0].occurred_time.Date;
                    dialog = new MessageDialog(Content, Title);
                    await dialog.ShowAsync();

                }
            }
            catch (Exception ex) { }

        }
        private void AllBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            ToggleButton CholeraBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "CholeraBtn") as ToggleButton;
            ToggleButton RotavirusBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "RotavirusBtn") as ToggleButton;
            ToggleButton SalmonellaBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "SalmonellaBtn") as ToggleButton;
            ToggleButton ShigellaBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "ShigellaBtn") as ToggleButton;
            ToggleButton OtherBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "OtherBtn") as ToggleButton;
            AllBtn.IsEnabled = false;
            CholeraBtn.IsChecked = false;
            RotavirusBtn.IsChecked = false;
            SalmonellaBtn.IsChecked = false;
            ShigellaBtn.IsChecked = false;
            OtherBtn.IsChecked = false;
            check_cholera = 0;
            check_other = 0;
            check_rotavirus = 0;
            check_salmonella = 0;
            check_shigella =  0;
            check_all = 1;
            loadDisease();
            loadDisaster();
            loadRF();
        }

        private void CholeraBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            AllBtn.IsChecked = false;
            AllBtn.IsEnabled = true;
            check_cholera = 1;
            check_all = 0;
            loadDisease();
            
        }

        private void RotavirusBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            AllBtn.IsChecked = false;
            AllBtn.IsEnabled = true;
            check_rotavirus = 1;
            check_all = 0;
            loadDisease();
            
        }

        private void SalmonellaBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            AllBtn.IsChecked = false;
            AllBtn.IsEnabled = true;
            check_salmonella = 1;
            check_all = 0;
            loadDisease();
            
        }

        private void ShigellaBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            AllBtn.IsChecked = false;
            AllBtn.IsEnabled = true;
            check_shigella = 1;
            check_all = 0;
            loadDisease();
            
        }

        private void OtherBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton AllBtn = FindChildControl<ToggleButton>(ResponsibleAreaSection, "AllBtn") as ToggleButton;
            AllBtn.IsChecked = false;
            AllBtn.IsEnabled = true;
            check_other = 1;
            check_all = 0;
            loadDisease();
            
        }
        private void OtherBtn_unClick(object sender, RoutedEventArgs e)
        {
            check_other = 0;
            check_all = 0;
            loadDisease();
            
        }

        private void ShigellaBtn_unClick(object sender, RoutedEventArgs e)
        {
            check_shigella = 0;
            loadDisease();
            
        }

        private void SalmonellaBtn_unClick(object sender, RoutedEventArgs e)
        {
            check_salmonella = 0;
            loadDisease();
            
        }

        private void RotavirusBtn_unClick(object sender, RoutedEventArgs e)
        {
            check_rotavirus = 0;
            loadDisease();
            
        }

        private void CholeraBtn_unClick(object sender, RoutedEventArgs e)
        {
            check_cholera = 0;
            loadDisease();
            
        }
        private void ProbabilitySider_Change(object sender, RangeBaseValueChangedEventArgs e)
        {
            loadDisease();
            if (check_all == 1)
            {
                loadDisaster();
                loadRF();
            }
        }
        private void StartDate_Change(object sender, DatePickerValueChangedEventArgs e)
        {
            loadDisease();
            if (check_all == 1)
            {
                loadDisaster();
                loadRF();
            }
            loadNum();
            loadgraph();
        }

        private void EndDate_Change(object sender, DatePickerValueChangedEventArgs e)
        {
            loadDisease();
            if (check_all == 1)
            {
                loadDisaster();
                loadRF();
            }
            loadNum();
            loadgraph();
        }
#endif
#if WINDOWS_PHONE_APP
        public async void InitializeMap()
        {

            myMap.MapServiceToken = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 17;


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
                var client = new HttpClient();
                foreach (Disease_Report report in reports)
                {
                            var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                            var response = await client.GetAsync(Uri);
                            var result = await response.Content.ReadAsStringAsync();
                            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                            var serializer = new DataContractJsonSerializer(typeof(RootObject));
                            var list = serializer.ReadObject(ms);
                            var jsonResponse = list as RootObject;
                            string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                            if (postcode == user_postcode)
                            {
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "p");

                            }
                }
                
            }
            catch (Exception e) { }
        }

        private async void loadDisaster()
        {
            try
            {
                var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
                var client = new HttpClient();
                foreach (Disaster_Report report in reports)
                {
                    var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    var response = await client.GetAsync(Uri);
                    var result = await response.Content.ReadAsStringAsync();
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    var serializer = new DataContractJsonSerializer(typeof(RootObject));
                    var list = serializer.ReadObject(ms);
                    var jsonResponse = list as RootObject;
                    string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;

                    if (postcode == user_postcode)
                    {
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "g");

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
                var client = new HttpClient();
                foreach (Risk_Factor_Report report in reports)
                {
                    var Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                    var response = await client.GetAsync(Uri);
                    var result = await response.Content.ReadAsStringAsync();
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                    var serializer = new DataContractJsonSerializer(typeof(RootObject));
                    var list = serializer.ReadObject(ms);
                    var jsonResponse = list as RootObject;
                    string postcode = jsonResponse.results[0].address_components[jsonResponse.results[0].address_components.Count - 1].long_name;
                    if (postcode == user_postcode)
                    {
                    AddPushpin(new BasicGeoposition() { Latitude = report.latitude, Longitude = report.longitude }, report.id, "b");

                    }
                }


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
            catch (Exception ex) { }

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
