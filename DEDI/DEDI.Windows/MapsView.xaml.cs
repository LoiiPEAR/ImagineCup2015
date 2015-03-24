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
        public MapsView()
        {
            this.InitializeComponent();
            InitializeMap();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

       public void InitializeMap()
        {

            double lat = 0;
            double lon = 0;
            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 17;
            myMap.MapType = MapType.Road;


            /* [{"Lat":"13.815361","Lon":"100.560822","Name":"Central Patpharo"},{"Lat":"13.81433","Lon":"100.560162","Name":"MRT Phaholyothin"}] */
            string strJSON = string.Empty;
            strJSON = " [{\"Lat\":\"13.815361\",\"Lon\":\"100.560822\",\"LocationName\":\"Central Patpharo\"},{\"Lat\":\"13.81433\",\"Lon\":\"100.560162\",\"LocationName\":\"MRT Phaholyothin\"}]";


            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJSON));
            ObservableCollection<Response> list = new ObservableCollection<Response>();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Response>));
            list = (ObservableCollection<Response>)serializer.ReadObject(ms);

            //foreach (Address loc in list)
            //{
            //    Pushpin pushpin = new Pushpin();
            //    pushpin.Tapped += new TappedEventHandler(pushpinTapped);
            //    pushpin.Name = loc.name;
            //    MapLayer.SetPosition(pushpin, new Location(loc.coordinates[0], loc.coordinates[1]));
            //    myMap.Children.Add(pushpin);

            //    lat = loc.coordinates[0];
            //    lon = loc.coordinates[1];
            //}

            //myMap.Center = new Location(lat, lon);

        }

        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(((Pushpin)sender).Name);
            await dialog.ShowAsync();
        }
    }
}
