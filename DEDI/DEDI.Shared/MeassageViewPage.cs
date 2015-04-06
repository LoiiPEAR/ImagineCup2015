using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace DEDI
{
    public sealed partial class MessageViewPage
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
            List<MessageItem> list = new List<MessageItem>();
            List<Message> msg = await App.MobileService.GetTable<Message>().Where(r => r.hw_receiver_id == user.id).ToListAsync();
            foreach(Message m in msg){
                List<Health_Worker> hw = await App.MobileService.GetTable<Health_Worker>().Where(h => h.id==m.hw_sender_id).ToListAsync();
                Health_Worker sender = hw[0];
                list.Add(new MessageItem() { topic = m.topic, content = m.content, sender = hw[0].fname + " " + hw[0].lname, sent_time = m.sent_time, sender_id = hw[0].id });
            }
            DefaultViewModel["Items"] = list;
        }
        
    }
    public class MessageItem
    {
        public string topic { get; set; }
        public string content { get; set; }
        public string sender { get; set; }
        public string sender_id { get; set; }
        public DateTime sent_time { get; set; }
        
    }
    
}
