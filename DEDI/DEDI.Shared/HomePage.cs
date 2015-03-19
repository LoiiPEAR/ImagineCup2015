using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class HomePage
    {
        private List<HealthWorker> hw;
        private IMobileServiceTable<HealthWorker> memberTable = App.MobileService.GetTable<HealthWorker>();
        public HomePage()
        {
            this.InitializeComponent();
           
        }

        private async void loadData()
        {
            try
            {
                hw = await memberTable.Where( user => user.Firstname =="Phanumas").Take(1).ToListAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                throw e;
            }


            string FirstName = hw[0].Firstname;
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
             loadData();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LogInPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage));
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
