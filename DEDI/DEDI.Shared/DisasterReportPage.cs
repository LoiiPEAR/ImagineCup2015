﻿#if WINDOWS_APP
    using Bing.Maps;
#endif
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
#if WINDOWS_PHONE_APP
using Windows.UI.Xaml.Controls.Maps;
#endif
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI;

namespace DEDI
{
    public sealed partial class DisasterReportPage
    {
        private string disaster = "";
#if WINDOWS_APP
        DraggablePin pin;
#endif
#if WINDOWS_PHONE_APP
        Grid pin;
#endif
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            InitializeMap();
            // Set the MinDate and MaxDate.
            DatePicker.MinYear = new DateTime(1985, 6, 20);
            DatePicker.MaxYear = DateTime.Today;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage),user);
        }
        private void FloodBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Flood";
            FloodBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/flood_btn_pressed.png"));
        }
        private void StromBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Storm";
            StormBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/storm_btn_pressed.png"));
        }
        private void WildFireBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Wildfire";
            WildFireBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/wildfire_btn_pressed.png"));
        }
        private void TsunamiBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Tsunami";
            TsunamiBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/tsunami_btn_pressed.png"));
        }
        private void EarthQuakeBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Earthquake";
            EarthQuakeBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/earthquake_btn_pressed.png"));
        }
        private void VolcanicEruptionBtn_Click(object sender, RoutedEventArgs e)
        {
            changeBG();
            disaster = "Volcanic eruption";
            VocanicEruptionBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/volcanic_btn_pressed.png"));
        }
        private void changeBG()
        {
            if (disaster == "Flood") FloodBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/flood_btn.png"));
            else if (disaster == "Wildfire") WildFireBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/wildfire_btn.png"));
            else if (disaster == "Storm") StormBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/storm_btn.png"));
            else if (disaster == "Volcanic eruption") VocanicEruptionBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/volcanic_btn.png"));
            else if (disaster == "Tsunami") TsunamiBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/tsunami_btn.png"));
            else if (disaster == "Earthquake") EarthQuakeBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/earthquake_btn.png"));
        }
        public async void InitializeMap()
        {
            try
            {
#if WINDOWS_APP
            myMap.Credentials = "AgRNfxvPZHZijcTen8d_YdjOczkXbxKLoIegltoNSdqhGqHmq5PpeZxDvyFw4HM6";
            myMap.ZoomLevel = 10;
            myMap.MapType = MapType.Road;


            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                          TimeSpan.FromSeconds(10));
            myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
            pin = new DraggablePin(myMap);
            //Set the location of the pin to the center of the map.
            Bing.Maps.MapLayer.SetPosition(pin, myMap.Center);

            //Set the pin as draggable.
            pin.Draggable = true;

            //Attach to the drag event of the pin.
            pin.DragEnd += Pin_Dragged;

            //Add the pin to the map.
            myMap.Children.Add(pin);
            myMap.PointerPressedOverride += myMap_PointerPressedOverride;
            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address;
#endif
#if WINDOWS_PHONE_APP
                myMap.MapServiceToken = "AgRNfxvPZHZijcTen8d_YdjOczkXbxKLoIegltoNSdqhGqHmq5PpeZxDvyFw4HM6";
                myMap.ZoomLevel = 10;

                pin = new Grid()
                {
                    Width = 30,
                    Height = 30,
                    Margin = new Windows.UI.Xaml.Thickness(-12)
                };


                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Blue),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });

                // Get my current location.
                Geolocator myGeolocator = new Geolocator();
                Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
                Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
                MapControl.SetLocation(pin, myGeocoordinate.Point);
                myMap.Center = myGeocoordinate.Point;
                myMap.Children.Add(pin);
#endif
            }
            catch (Exception e) { }
        }

        private async void myMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            try
            {
#if WINDOWS_APP
            var pointerPosition = e.GetCurrentPoint(((Map)sender));

            Bing.Maps.Location location = null;

            //Convert the point pixel to a Location coordinate
            if (((Map)sender).TryPixelToLocation(pointerPosition.Position, out location))
            {
                Bing.Maps.MapLayer.SetPosition(pin, location);
                var client = new HttpClient();
                Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + location.Latitude + "," + location.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                RootObject jsonResponse = list as RootObject;
                AddressTB.Text = jsonResponse.results[0].formatted_address;
           }
#endif
            }
            catch (Exception ex) { }
        }
#if WINDOWS_PHONE_APP
        private async void MapControl_MapTapped(MapControl sender, MapInputEventArgs args)
        {
            try
            {
                var client = new HttpClient();
                Geopoint location = args.Location;
                MapControl.SetLocation(pin, location);
                Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + location.Position.Latitude + "," + location.Position.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                RootObject jsonResponse = list as RootObject;
                AddressTB.Text = jsonResponse.results[0].formatted_address;
            }
            catch (Exception e) { }
        }
#endif



#if WINDOWS_APP
        private async void Pin_Dragged(Bing.Maps.Location obj)
        {

            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + obj.Latitude + "," + obj.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address; 
        }
#endif
        private async void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (disaster == "")
                {
                    Type_errorTbl.Text = "Please select type of disaster";
                }
                else
                {

                    SubmitBtn.IsEnabled = false;
                    Disaster_Report d = new Disaster_Report()
                    {
                        disaster = this.disaster,
                        description = DescriptionTb.Text,
#if WINDOWS_APP
                    latitude = Bing.Maps.MapLayer.GetPosition(pin).Latitude,
                    longitude = Bing.Maps.MapLayer.GetPosition(pin).Longitude,
#endif
#if WINDOWS_PHONE_APP
                        longitude = MapControl.GetLocation(pin).Position.Longitude,
                        latitude = MapControl.GetLocation(pin).Position.Latitude,
#endif
                        occurred_time = DatePicker.Date.UtcDateTime,
                        hw_id = user.id,
                        reported_time = DateTime.Now.Date
                    };
#if WINDOWS_PHONE_APP
                    Disaster_Report d = new Disaster_Report()
                    {
                        disaster = this.disaster,
                        description = DescriptionTb.Text,
                        longitude = MapControl.GetLocation(pin).Position.Longitude,
                        latitude = MapControl.GetLocation(pin).Position.Latitude,

                        occurred_time = DatePicker.Date.UtcDateTime,
                        hw_id = user.id,
                        reported_time = DateTime.Now.Date
                    };
#endif
                    IMobileServiceTable<Disaster_Report> hwTable = App.MobileService.GetTable<Disaster_Report>();
                    await hwTable.InsertAsync(d);
                    this.Frame.Navigate(typeof(ReportsView), user);

                }
            }
            catch (Exception ex) { }
        }
    }
}
