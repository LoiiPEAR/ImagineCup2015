﻿using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class MyReportPage
    {
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            loadDisaster();
            loadRF();
            loadDisease();
        }

        private async void loadDisaster()
        {
            var reports = await App.MobileService.GetTable<Disaster_Report>().Where(r => r.hw_id == user.id).ToListAsync();
            ImageSource src = null;
            List<Disaster_Report_View> disasterView = new List<Disaster_Report_View>();
            if (reports.Count > 0)
            {
                foreach (Disaster_Report item in reports)
                {
                    if (item.disaster != "")
                    {
                        if (item.disaster.Equals("Earthquake")) src = new BitmapImage(new Uri("ms-appx:/Assets/earthquake_btn.png"));
                        else if (item.disaster.Equals("Flood")) src = new BitmapImage(new Uri("ms-appx:/Assets/flood_btn.png"));
                        else if (item.disaster.Equals("Storm")) src = new BitmapImage(new Uri("ms-appx:/Assets/storm_btn.png"));
                        else if (item.disaster.Equals("Wildfire")) src = new BitmapImage(new Uri("ms-appx:/Assets/wildfire_btn.png"));
                        else if (item.disaster.Equals("Tsunami")) src = new BitmapImage(new Uri("ms-appx:/Assets/tsunami_btn.png"));
                        else if (item.disaster.Equals("Volcanic eruption")) src = new BitmapImage(new Uri("ms-appx:/Assets/volcanic_btn.png"));
                        disasterView.Add(new Disaster_Report_View
                        {
                            disaster = item.disaster,
                            hw_id = item.hw_id,
                            latitude = item.latitude,
                            longitude = item.longitude,
                            description = item.description,
                            reported_time = item.reported_time,
                            ocurred_time = item.ocurred_time,
                            icon = src
                        });

                    }
                }
                DisasterReportsViewLv.ItemsSource = disasterView;
                DisasterReportsViewLv.SelectionMode = ListViewSelectionMode.None;
            }

        }
        private async void loadRF()
        {
            var reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.hw_id == user.id).ToListAsync();
            ImageSource src = null;
            List<Risk_Factor_Report_View> riskView = new List<Risk_Factor_Report_View>();
            if (reports.Count > 0)
            {
                foreach (Risk_Factor_Report item in reports)
                {
                    if (item.risk_factor != "")
                    {
                        if (item.risk_factor.Equals("Contaminated food")) src = new BitmapImage(new Uri("ms-appx:/Assets/food_btn.png"));
                        else if (item.risk_factor.Equals("Contaminated water")) src = new BitmapImage(new Uri("ms-appx:/Assets/water_btn.png"));
                        else if (item.risk_factor.Equals("Crowding")) src = new BitmapImage(new Uri("ms-appx:/Assets/crowding_btn.png"));
                        else if (item.risk_factor.Equals("Poor sanitation")) src = new BitmapImage(new Uri("ms-appx:/Assets/sanitation_btn.png"));
                        riskView.Add(new Risk_Factor_Report_View
                        {
                            risk_factor = item.risk_factor,
                            hw_id = item.hw_id,
                            latitude = item.latitude,
                            longitude = item.longitude,
                            description = item.description,
                            reported_time = item.reported_time,
                            ocurred_time = item.ocurred_time,
                            icon = src
                        });

                    }
                }
                RiskFactorReportsViewLv.ItemsSource = riskView;
                RiskFactorReportsViewLv.SelectionMode = ListViewSelectionMode.None;
            }
        }
        private async void loadDisease()
        {
            var reports = await App.MobileService.GetTable<Disease_Report>().Where(r => r.hw_id == user.id).ToListAsync();
            ImageSource src = null;
            List<Disease_Report_View> diseaseView = new List<Disease_Report_View>();
            if (reports.Count > 0)
            {
                foreach (Disease_Report item in reports)
                {
                    if (item.id != "") DiseaseReportsViewLv.ItemsSource = reports;
                    src = new BitmapImage(new Uri("ms-appx:/Assets/disease_report_btn.png"));
                    diseaseView.Add(new Disease_Report_View
                    {
                        id = "Disease report",
                        hw_id = item.hw_id,
                        latitude = item.latitude,
                        longitude = item.longitude,
                        description = item.description,
                        reported_time = item.reported_time,
                        ocurred_time = item.ocurred_time,
                        icon = src
                    });
                }
                DiseaseReportsViewLv.ItemsSource = diseaseView;
                DiseaseReportsViewLv.SelectionMode = ListViewSelectionMode.None;
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), user);
        }
    }
}