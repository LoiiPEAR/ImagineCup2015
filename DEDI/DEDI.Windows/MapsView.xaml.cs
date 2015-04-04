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
namespace DEDI
{
    public sealed partial class MapsView:Page
    {
        Health_Worker user;
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

            loadRF();
            loadDisaster();


        }

       private async void loadDisaster()
       {
           var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
           foreach (Disaster_Report report in reports)
           {
               Pushpin pushpin = new Pushpin();
               pushpin.Tapped += new TappedEventHandler(pushpinTapped);
               pushpin.Name = report.disaster;
               MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude,report.longitude));
               myMap.Children.Add(pushpin);
           }
            
       }
       private async void loadRF()
       {
           var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
           foreach (Risk_Factor_Report report in reports)
           {
               Pushpin pushpin = new Pushpin();
               pushpin.Tapped += new TappedEventHandler(pushpinTapped);
               pushpin.Name = report.risk_factor;
               MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
               myMap.Children.Add(pushpin);
           }

       }
       private async void loadDisease()
       {
           

       }
        
        

        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(((Pushpin)sender).Name);
            await dialog.ShowAsync();
        }
    }
}
