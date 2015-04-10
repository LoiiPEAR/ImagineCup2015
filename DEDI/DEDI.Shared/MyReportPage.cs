using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
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
            if (reports.Count > 0)
            {
                foreach (Disaster_Report item in reports)
                {
                    if (item.disaster != "") DisasterReportsViewLv.ItemsSource = reports;
                    //foreach (Disaster_Report item in DisasterReportsViewLv.Items)
                    //{
                    //    if (item.disaster == "Flood") item.icon = new BitmapImage(new Uri("ms-appx:/Assets/flood_btn.png"));
                    //}
                }
            }

        }
        private async void loadRF()
        {
            var reports = await App.MobileService.GetTable<Risk_Factor_Report>().Where(r => r.hw_id == user.id).ToListAsync();
            if (reports.Count > 0)
            {
                foreach (Risk_Factor_Report item in reports)
                {
                    if (item.risk_factor != "") RiskFactorReportsViewLv.ItemsSource = reports;
                }
            }
        }
        private async void loadDisease()
        {
            var reports = await App.MobileService.GetTable<Disease_Report>().Where(r => r.hw_id == user.id).ToListAsync();
            if (reports.Count > 0)
            {
                foreach (Disease_Report item in reports)
                {
                    if (item.id != "") DiseaseReportsViewLv.ItemsSource = reports;
                }
            }

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), user);
        }
    }
}
