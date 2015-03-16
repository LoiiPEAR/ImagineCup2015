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
#if WINDOWS_APP
using Bing.Maps;
#endif
#if WINDOW_PHONE_APP
using Microsift.Phone.Maps.Contorls;
using IntelliFactory.WebSharper.Bing;
#endif

using Windows.UI.Popups;

using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Text;
namespace DEDI
{
    public sealed partial class MapsView
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

        void InitializeMap()
        {

            double lat = 0;
            double lon = 0;
            //Map myMap = new Map();
            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 17;
            myMap.MapType = MapType.Road;
            myMap.Width = 800;
            myMap.Height = 800;

            /* [{"Lat":"13.815361","Lon":"100.560822","LocationName":"Central Patpharo"},{"Lat":"13.81433","Lon":"100.560162","LocationName":"MRT Phaholyothin"}] */
            string strJSON = string.Empty;
            strJSON = " [{\"Lat\":\"13.815361\",\"Lon\":\"100.560822\",\"LocationName\":\"Central Patpharo\"},{\"Lat\":\"13.81433\",\"Lon\":\"100.560162\",\"LocationName\":\"MRT Phaholyothin\"}]";


            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJSON));
            ObservableCollection<Address> list = new ObservableCollection<Address>();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Address>));
            list = (ObservableCollection<Address>)serializer.ReadObject(ms);

            foreach (Address loc in list)
            {
                Pushpin pushpin = new Pushpin();
                pushpin.Tapped += new TappedEventHandler(pushpinTapped);
                pushpin.Name = loc.LocationName;
                MapLayer.SetPosition(pushpin, new Location(loc.Lat, loc.Lon));
                myMap.Children.Add(pushpin);

                lat = loc.Lat;
                lon = loc.Lon;
            }

            myMap.Center = new Location(lat, lon);

        }

        private async void pushpinTapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(((Pushpin)sender).Name);
            await dialog.ShowAsync();
        }
    }
}
