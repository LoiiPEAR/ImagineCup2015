using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Microsoft.WindowsAzure.MobileServices;

namespace DEDI
{
    public sealed partial class FollowingPage
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
            List<Health_Worker> hw = new List<Health_Worker>();
            List<Follow> follows = await App.MobileService.GetTable<Follow>().Where(follower => follower.follower_hw_id==user.id).ToListAsync();
            foreach (Follow follow in follows)
            {
                List<Health_Worker> followingHW = await App.MobileService.GetTable<Health_Worker>().Where(u => u.id == follow.following_hw_id).ToListAsync();
                foreach (Health_Worker following in followingHW)
                {
                    hw.Add(following);
                }
            }
            
            var h = hw.GroupBy(u => u.position).OrderBy(t => t.Key.ToString()); ;
#if WINDOWS_APP
            DefaultViewModel["Groups"] = h;
#endif
        }
        private async void AddFollow_Click(object sender, RoutedEventArgs e)
        {
            List<Health_Worker> hw_list = new List<Health_Worker>();
            List<Health_Worker> h = await App.MobileService.GetTable<Health_Worker>().Where( hw => hw.id!=user.id).ToListAsync();
            foreach (Health_Worker hw in h)
            {
                List<Follow> followingHW = await App.MobileService.GetTable<Follow>().Where(follower => follower.following_hw_id == hw.id).ToListAsync();
                if (followingHW.Count == 0) hw_list.Add(hw);
            }

            HealthWorkerLV.ItemsSource = hw_list;
        }
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            Health_Worker h = HealthWorkerLV.SelectedItem as Health_Worker;
            Follow f = new Follow()
            {
                following_hw_id = h.id,
                follower_hw_id = user.id
            };
            IMobileServiceTable<Follow> followTable = App.MobileService.GetTable<Follow>();
            await followTable.InsertAsync(f);
            followFlyout.Hide();
            loadContract();
        }
        private async void UnfollowBtn_Click(object sender, RoutedEventArgs e)
        {
            //Follow following = ;
            //List<Follow> followingHW = await App.MobileService.GetTable<Follow>().Where(follower => follower.following_hw_id == following.id && follower.follower_hw_id==user.id).ToListAsync();
            //IMobileServiceTable<Follow> followTable = App.MobileService.GetTable<Follow>();
            //await followTable.DeleteAsync(followingHW[0]);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            followFlyout.Hide();
        }
        
    }
    
}
