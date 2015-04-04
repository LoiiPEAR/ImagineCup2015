﻿using Bing.Maps;
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
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class RiskFactorReportPage
    {
        Health_Worker user;
        DraggablePin pin;
        private string risk_factor = "";
        public RiskFactorReportPage()
        {
            InitializeComponent();
        }
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
        private async void InitializeMap()
        {

            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
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
            Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
            var list = serializer.ReadObject(ms);
            Response jsonResponse = list as Response;
            AddressTB.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;

        }

        private void ContraminatedWaterBtn_Click(object sender, RoutedEventArgs e)
        {
            risk_factor = "Contaminated water";
        }

        private void ContraminatedFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            risk_factor = "Contaminated food";
        }

        private void CrowdingBtn_Click(object sender, RoutedEventArgs e)
        {
            risk_factor = "Crowding";
        }

        private void PoorsanitationBtn_Click(object sender, RoutedEventArgs e)
        {
            risk_factor = "Poor sanitation";

        }
        private async void myMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
            var pointerPosition = e.GetCurrentPoint(((Map)sender));

            Bing.Maps.Location location = null;

            //Convert the point pixel to a Location coordinate
            if (((Map)sender).TryPixelToLocation(pointerPosition.Position, out location))
            {
                Bing.Maps.MapLayer.SetPosition(pin, location);
                var client = new HttpClient();
                Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + location.Latitude + "," + location.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
                var response = await client.GetAsync(uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
                var list = serializer.ReadObject(ms);
                Response jsonResponse = list as Response;
                ms.Flush();
                if (jsonResponse.ResourceSets[0].EstimatedTotal != 0)
                    AddressTB.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
            }
        }





        private async void Pin_Dragged(Bing.Maps.Location obj)
        {

            var client = new HttpClient();
            Uri uri = new Uri("http://dev.virtualearth.net/REST/v1/Locations/" + obj.Latitude + "," + obj.Longitude + "?o=&key=AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht");
            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Response));
            var list = serializer.ReadObject(ms);
            Response jsonResponse = list as Response;
            ms.Flush();
            if (jsonResponse.ResourceSets[0].EstimatedTotal != 0)
                AddressTB.Text = jsonResponse.ResourceSets[0].Resources[0].Address.FormattedAddress;
        }
        private async void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (risk_factor == "")
            {
                Type_errorTbl.Text = "Please select type of risk factor";
            }
            else
            {
                Risk_Factor_Report r = new Risk_Factor_Report()
                {
                    risk_factor = this.risk_factor,
                    description = DescriptionTb.Text,
                    latitude = Bing.Maps.MapLayer.GetPosition(pin).Latitude,
                    longitude = Bing.Maps.MapLayer.GetPosition(pin).Longitude,
                    ocurred_time = DatePicker.Date.UtcDateTime,
                    hw_id = user.id,
                    reported_time = DateTime.Now.Date
                };
                IMobileServiceTable<Risk_Factor_Report> hwTable = App.MobileService.GetTable<Risk_Factor_Report>();
                await hwTable.InsertAsync(r);
                this.Frame.Navigate(typeof(ReportsView), user);
            }
        }
    }
}
