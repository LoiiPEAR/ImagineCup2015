using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace DEDI
{
    public sealed partial class HomePage
    {
        private List<Health_Worker> hw;
        //private MobileServiceCollection<Health_Worker, Health_Worker> items;
        private IMobileServiceTable<Health_Worker> memberTable;
        public HomePage()
        {
            this.InitializeComponent();
            
        }

        private async void loadData()
        {
            try
            {
                //items = await memberTable.ToCollectionAsync();
                var getData = await App.MobileService.GetTable<Health_Worker>().ToCollectionAsync();
                int i = 0;
                //hw = await memberTable.ToListAsync();
                //Health_Worker test = hw[0];
                //string _Firstname = test.fname;

            }
            catch (MobileServiceInvalidOperationException e)
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
                MessageDialog ms = new MessageDialog(e.ToString());
                ms.ShowAsync();
            }


            
            
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            loadData();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }

        private void CreateReportBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage));
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashBoard));
        }

        private void GoToMapBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapsView));
        }

        private void GoToReportBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReportsView));
        }
    }
}
