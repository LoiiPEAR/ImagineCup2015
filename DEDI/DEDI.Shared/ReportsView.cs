using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class ReportsView
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
            var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();
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
            var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
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
            var reports = await App.MobileService.GetTable<Disease_Report>().ToListAsync();
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

        //private void loadFilter()
        //{
        //    FilterCb.Items.Add("Disaster Report");
        //    FilterCb.Items.Add("Disease Report");
        //    FilterCb.Items.Add("Risk Factor Report");
        //    FilterCb.SelectedIndex = FilterCb.Items.IndexOf("Disaster Report");
        //}
        //private void Change_Filter(object sender, SelectionChangedEventArgs e)
        //{
        //    //ComboBoxItem typeItem = (ComboBoxItem)FilterCb.SelectedItem;
        //    string type = FilterCb.SelectedItem.ToString();
        //    if (type.Equals("Disaster Report")) loadDisaster();
        //    else if (type.Equals( "Disease Report")) loadDisease();
        //    else if (type.Equals("Risk Factor Report")) loadRF();
        //}

        
        
    }
}
