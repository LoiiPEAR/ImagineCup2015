using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class ReportsView
    {
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            loadFilter();
        }

        private async void loadDisaster()
        {
            var reports = await App.MobileService.GetTable<Disaster_Report>().ToListAsync();

            DisasterReportsViewLv.ItemsSource = reports;

        }
        private async void loadRF()
        {
            var reports = await App.MobileService.GetTable<Risk_Factor_Report>().ToListAsync();
            RiskFactorReportsViewLv.ItemsSource = reports;
           

        }
        private async void loadDisease()
        {


        }

        private void loadFilter()
        {
            FilterCb.Items.Add("Disaster Report");
            FilterCb.Items.Add("Disease Report");
            FilterCb.Items.Add("Risk Factor Report");
            FilterCb.SelectedIndex = FilterCb.Items.IndexOf("Disaster Report");
        }
        private void Change_Filter(object sender, SelectionChangedEventArgs e)
        {
            //ComboBoxItem typeItem = (ComboBoxItem)FilterCb.SelectedItem;
            string type = FilterCb.SelectedItem.ToString();
            if (type.Equals("Disaster Report")) loadDisaster();
            else if (type.Equals( "Disease Report")) loadDisease();
            else if (type.Equals("Risk Factor Report")) loadRF();
        }

        
        
    }
}
