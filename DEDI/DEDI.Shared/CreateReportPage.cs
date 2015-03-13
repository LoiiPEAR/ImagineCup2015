using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class CreateReportPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }

        private void DisasterBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DisasterReportPage));
        }

        private void DiseaseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DiseaseReportPage));
        }

        private void RiskFactorBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RiskFactorReportPage));
        }
    }
}
