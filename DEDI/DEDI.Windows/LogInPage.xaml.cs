

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace DEDI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {
        public LogInPage()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
        {
            this.InitializeComponent();
        }

        private void gotoMap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapsView));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == "Phanumas" && hw.password == ComputeMD5("ppB023056123")).ToListAsync();
            this.Frame.Navigate(typeof(ContractPage),user[0]);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == "Loii.PEAR" && hw.password == ComputeMD5("z,iyd86I")).ToListAsync();
            this.Frame.Navigate(typeof(MessageViewPage), user[0]);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var user = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.username == "Phanumas" && hw.password == ComputeMD5("ppB023056123")).ToListAsync();
            this.Frame.Navigate(typeof(SendMessagePage), user[0]);
        }
       
    }
}
