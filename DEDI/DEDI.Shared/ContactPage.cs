using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Navigation;


namespace DEDI
{
    public sealed partial class ContractPage
    {
        Health_Worker user;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            user = e.Parameter as Health_Worker;
            loadContract();
        }

        private async void loadContract()
        {

            List<Health_Worker> hw = await App.MobileService.GetTable<Health_Worker>().ToListAsync();
            //List<GroupInfoList<object>> Groups = new List<GroupInfoList<object>>();
            // GroupInfoList<object> info = new GroupInfoList<object>();
            //info.Key =hw[0].position;
            //foreach (Health_Worker h in hw)
            //{
            //    if (h.position == info.Key)
            //    {
            //        info.Add(h);
            //    }
            //    else
            //    {
            //        Groups.Add(info);
            //        info = new GroupInfoList<object>();
            //        info.Key = h.position;
            //        info.Add(h);
            //    }
            //}
            //Groups.Add(info);

            var h = hw.GroupBy(u => u.position).OrderBy(t => t.Key.ToString()); ;
            DefaultViewModel["Groups"] = h;
        }
        
        
    }
}
