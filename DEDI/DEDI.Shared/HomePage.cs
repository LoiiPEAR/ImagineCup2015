using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;


namespace DEDI
{
    public sealed partial class HomePage
    {
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
