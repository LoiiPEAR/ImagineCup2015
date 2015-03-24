using Bing.Maps;
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

namespace DEDI
{
    public sealed partial class HomePage
    {
        public HomePage()
        {
            this.InitializeComponent();
            
        }

        private async void loadData(Health_Worker user)
        {
            try
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                               TimeSpan.FromSeconds(10));
               
                var client = new HttpClient();
                Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude+ "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                var list = serializer.ReadObject(ms);
                Response jsonResponse = list as Response;
                
                
                TextBlock FirstNameTbl = FindChildControl<TextBlock>(ProfileSection, "FirstNameTbl") as TextBlock;
                FirstNameTbl.Text = user.fname;
                TextBlock LastNameTbl = FindChildControl<TextBlock>(ProfileSection, "LastNameTbl") as TextBlock;
                LastNameTbl.Text = user.lname;
                TextBlock LocationTbl = FindChildControl<TextBlock>(ProfileSection, "LocationTbl") as TextBlock;
                LocationTbl.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
                TextBlock StatusTbl = FindChildControl<TextBlock>(ProfileSection, "StatusTbl") as TextBlock;
                StatusTbl.Text = "";
               
                Map myMap = FindChildControl<Map>(MapSection, "myMap") as Map;
                myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                myMap.ZoomLevel = 17;
                myMap.MapType = MapType.Road;
                myMap.Width = 420;
                myMap.Height = 480;
                myMap.Center = new Bing.Maps.Location(jsonResponse.ResourceSets[0].Resources[0].Point.Coordinates[0], jsonResponse.ResourceSets[0].Resources[0].Point.Coordinates[1]);

            }
            catch (MobileServiceInvalidOperationException e)
            {
                MessageDialog ms = new MessageDialog(e.ToString());
                ms.ShowAsync();
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
            Health_Worker hw = e.Parameter as Health_Worker;
            loadData(hw);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage));
        }

        private void GoToMapBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapsView));
        }

        private void GoToReportBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReportsView));
        }
    }
}
