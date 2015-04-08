using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;


namespace DEDI
{
    public sealed partial class ContractPage
    {
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //navigationHelper.OnNavigatedTo(e);
            user = e.Parameter as Health_Worker;
            loadContract();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage), user);
        }

        private async void loadContract()
        {

            List<Health_Worker> hw = await App.MobileService.GetTable<Health_Worker>().ToListAsync();
           
            var h = hw.GroupBy(u => u.position).OrderBy(t => t.Key.ToString()); ;
            //DefaultViewModel["Groups"] = h;
        }
        
        
    }
}
