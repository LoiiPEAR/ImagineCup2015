using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class DashBoard
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
