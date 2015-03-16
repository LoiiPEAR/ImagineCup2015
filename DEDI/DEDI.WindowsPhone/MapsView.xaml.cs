using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Toolkit;
using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;

using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DEDI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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

       public void InitializeMap()
        {

            //*** Map
            Map MyMap = new Map();
            MyMap.ZoomLevel = 16;

            MapLayer layer = new MapLayer();

            /* [{"Lat":"13.815361","Lon":"100.560822","LocationName":"Central Patpharo"},{"Lat":"13.81433","Lon":"100.560162","LocationName":"MRT Phaholyothin"}] */
            string strJSON = string.Empty;
            strJSON = " [{\"Lat\":\"13.815361\",\"Lon\":\"100.560822\",\"LocationName\":\"Central Patpharo\"},{\"Lat\":\"13.81433\",\"Lon\":\"100.560162\",\"LocationName\":\"MRT Phaholyothin\"}]";

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJSON));
            ObservableCollection<MyLocation> list = new ObservableCollection<MyLocation>();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<MyLocation>));
            list = (ObservableCollection<MyLocation>)serializer.ReadObject(ms);

            foreach (MyLocation loc in list)
            {
                Pushpin pushpin1 = new Pushpin();
                pushpin1.GeoCoordinate = new GeoCoordinate(loc.Lat, loc.Lon);
                pushpin1.Background = new SolidColorBrush(Colors.Orange);
                MapOverlay overlay1 = new MapOverlay();
                overlay1.Content = pushpin1;
                overlay1.GeoCoordinate = new GeoCoordinate(loc.Lat, loc.Lon);
                layer.Add(overlay1);

                MyMap.Center = new GeoCoordinate(loc.Lat, loc.Lon);
            }


            // Map Layer
            MyMap.Layers.Add(layer);

            // CarphicMode
            MyMap.CartographicMode = MapCartographicMode.Hybrid;

            // Add map to display
            ContentPanel.Children.Add(MyMap);



        }

        
    }
}
