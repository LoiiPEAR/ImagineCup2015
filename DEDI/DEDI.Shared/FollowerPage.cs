using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

namespace DEDI
{
    public sealed partial class FollowerPage
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
            try
            {
                List<Health_Worker> hw = new List<Health_Worker>();
                List<Follow> follows = await App.MobileService.GetTable<Follow>().Where(follower => follower.following_hw_id == user.id).ToListAsync();
                foreach (Follow follow in follows)
                {
                    List<Health_Worker> followingHW = await App.MobileService.GetTable<Health_Worker>().Where(u => u.id == follow.follower_hw_id).ToListAsync();
                    foreach (Health_Worker following in followingHW)
                    {
                        hw.Add(following);
                    }
                }

                var h = hw.GroupBy(u => u.position).OrderBy(t => t.Key.ToString());
#if WINDOWS_APP
                DefaultViewModel["Groups"] = h;
#endif
            }
            catch (Exception e) { }
        }

    }

}
