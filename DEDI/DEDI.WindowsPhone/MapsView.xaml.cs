

using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DEDI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapsView
    {
        private Health_Worker user;
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
            //double lat = 0;
            //double lon = 0;
            //myMap.MapServiceToken = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            //myMap.ZoomLevel = 17;
            //myMap.Width = 300;
            //myMap.Height = 800;
            //myMap.LandmarksVisible = true;
            ///* [{"Lat":"13.815361","Lon":"100.560822","LocationName":"Central Patpharo"},{"Lat":"13.81433","Lon":"100.560162","LocationName":"MRT Phaholyothin"}] */
            //string strJSON = string.Empty;
            //strJSON = " [{\"Lat\":\"13.815361\",\"Lon\":\"100.560822\",\"LocationName\":\"Central Patpharo\"},{\"Lat\":\"13.81433\",\"Lon\":\"100.560162\",\"LocationName\":\"MRT Phaholyothin\"}]";

            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJSON));
            //ObservableCollection<Response> list = new ObservableCollection<Response>();
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<Response>));
            //list = (ObservableCollection<Response>)serializer.ReadObject(ms);

            //foreach (Response loc in list)
            //{
            //    AddPushpin(new BasicGeoposition() { Latitude=loc.Lat,Longitude=loc.Lon}, loc.LocationName);
            //    lat = loc.Lat;
            //    lon = loc.Lon;
            //}

            //myMap.Center = new Geopoint(new BasicGeoposition() { Latitude=lat,Longitude=lon});
           


        }
        public void AddPushpin(BasicGeoposition location, string text)
        {
            var pin = new Grid()
            {
                Width = 50,
                Height = 50,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 50,
                Height = 50
            });

            pin.Children.Add(new TextBlock()
            {
                Text = text,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
            });
            
            MapControl.SetLocation(pin, new Geopoint(location));
           
            myMap.Children.Add(pin);
        }

        private void backButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
        }

        
    }
}
