using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class CreateReportPage
    {
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
        }

        private void DisasterBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DisasterReportPage),user);
        }

        private void DiseaseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DiseaseReportPage),user);
        }

        private void RiskFactorBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RiskFactorReportPage),user);
        }
    }
}
