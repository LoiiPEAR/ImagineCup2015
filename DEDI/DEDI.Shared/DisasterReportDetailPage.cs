#if WINDOWS_APP
using Bing.Maps;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class DisasterReportDetailPage
    {
        Health_Worker user;

        Disaster_Report_View report;
        DisasterReportDetail reportdetail;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            reportdetail = e.Parameter as DisasterReportDetail;
            loaddata();
            InitializeMap();
        }
        private async void loaddata()
        {
            user = reportdetail.hw;
            report = reportdetail.report;
            DescriptionTb.Text = report.description;
            dateTb.Text = report.ocurred_time + "";
            DisasterImage.Source = report.icon;
            TypeTb.Text = report.disaster;
            var hw = await App.MobileService.GetTable<Health_Worker>().Where(r => r.id == report.hw_id).ToListAsync();
            NameTb.Text = hw[0].fname + " " + hw[0].lname;

        }
        private async void InitializeMap()
        {
#if WINDOWS_APP
            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 17;
            myMap.MapType = MapType.Road;

            Pushpin pushpin = new Pushpin();
            pushpin.Background = new SolidColorBrush(Colors.Blue);
            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
            myMap.Children.Add(pushpin);
            myMap.Center = new Bing.Maps.Location(report.latitude, report.longitude);


            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address;
#endif
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReportsView), user);
        }
    }
    class DisasterReportDetail
    {
        public Health_Worker hw { get; set; }
        public Disaster_Report_View report { get; set; }
    }
}