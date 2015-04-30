using System;
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
            try
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
                }
            }
            catch (Exception e) { }

        }
        private async void loadRF()
        {
            try
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
                }
            }
            catch (Exception e) { }
        }
        private async void loadDisease()
        {
            try
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
                            id = item.id,
                            type = "Disase Report",
                            hw_id = item.hw_id,
                            latitude = item.latitude,
                            longitude = item.longitude,
                            description = item.description,
                            reported_time = item.reported_time,
                            ocurred_time = item.ocurred_time,
                            icon = src,
                            patient_id = item.patient_id,
                            cholera = item.cholera,
                            shigella = item.shigella,
                            simolnelle = item.salmonella,
                            others = item.others,
                            rotavirus = item.rotavirus
                        });
                    }
                    DiseaseReportsViewLv.ItemsSource = diseaseView;
                }
            }
            catch (Exception e) { }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), user);
        }
        private void DisasterReportsViewLv_ItemClick(object sender, SelectionChangedEventArgs e)
        {

            Disaster_Report_View disaster = DisasterReportsViewLv.SelectedItem as Disaster_Report_View;
            DisasterReportDetail r = new DisasterReportDetail()
            {
                hw = user,
                report = disaster
            };
            this.Frame.Navigate(typeof(DisasterReportDetailPage), r);
        }
        private void RiskFactorReportsViewLv_ItemClick(object sender, SelectionChangedEventArgs e)
        {

            Risk_Factor_Report_View riskfactor = RiskFactorReportsViewLv.SelectedItem as Risk_Factor_Report_View;
            RiskFactorReportDetail r = new RiskFactorReportDetail()
            {
                hw = user,
                report = riskfactor
            };
            this.Frame.Navigate(typeof(MyRiskFactorDetailPage), r);
        }
        private void DiseaseReportsViewLv_ItemClick(object sender, SelectionChangedEventArgs e)
        {
            Disease_Report_View disease = DiseaseReportsViewLv.SelectedItem as Disease_Report_View;
            DiseaseReportDetail r = new DiseaseReportDetail()
            {
                hw = user,
                report = disease
            };
            this.Frame.Navigate(typeof(MyDiseaseReportDetailPage), r);
        }
    }
}
