using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Notifications;
using Microsoft.WindowsAzure.MobileServices;

namespace DEDI
{
    public sealed partial class MessageViewPage
    {
        Health_Worker user;
        Health_Worker rec;
        List<Health_Worker> contact;
        string status = "normal";
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            user = e.Parameter as Health_Worker;
            loadContact();
            loadContract();
        }

        private async void loadContract()
        {
            List<MessageItem> list = new List<MessageItem>();
            List<Message> msg = await App.MobileService.GetTable<Message>().Where(r => r.hw_receiver_id == user.id).ToListAsync();
            foreach(Message m in msg){
                List<Health_Worker> hw = await App.MobileService.GetTable<Health_Worker>().Where(h => h.id==m.hw_sender_id).ToListAsync();
                Health_Worker sender = hw[0];
                list.Add(new MessageItem() { topic = m.topic, content = m.content, sender = hw[0].fname + " " + hw[0].lname + " (" + hw[0].position + ")", sent_time = m.sent_time, sender_id = hw[0].id, status = m.status });
            }
            itemsViewSource.Source = list;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendGrid.Visibility = Visibility.Visible;
            itemDetailTitlePanel.Visibility = Visibility.Collapsed;
        }

        private void itemListView_GotFocus(object sender, RoutedEventArgs e)
        {
            sendGrid.Visibility = Visibility.Collapsed;
            itemDetailTitlePanel.Visibility = Visibility.Visible;
        }

        private async void loadContact()
        {
            contact = await App.MobileService.GetTable<Health_Worker>().Where(hw => hw.id != user.id).ToListAsync();

        }
        private void ReceiverTb_TextChange(object sender, TextChangedEventArgs e)
        {
            string typed_str = ReceiverTb.Text;
            List<Health_Worker> suggestion = new List<Health_Worker>();
            foreach (Health_Worker h in contact)
            {
                if (!string.IsNullOrEmpty(typed_str))
                {
                    if (h.fname.StartsWith(typed_str, StringComparison.OrdinalIgnoreCase))
                    {
                        suggestion.Add(h);
                    }
                }
            }
            if (suggestion.Count > 0)
            {
                AutocompleteLb.ItemsSource = suggestion;
                AutocompleteLb.Visibility = Visibility.Visible;
            }
            else if (ReceiverTb.Text.Equals(""))
            {
                AutocompleteLb.Visibility = Visibility.Collapsed;
                AutocompleteLb.ItemsSource = null;

            }
            else
            {
                AutocompleteLb.Visibility = Visibility.Collapsed;
                AutocompleteLb.ItemsSource = null;
                Reciever_errorTbl.Text = "Receiver is invalid";
            }
        }
        private void AutocompleteLb_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            AutocompleteLb.Visibility = Visibility.Collapsed;
            if (AutocompleteLb.SelectedIndex != -1)
            {
                rec = ((Health_Worker)AutocompleteLb.SelectedItem);
                ReceiverTb.Text = rec.fname + " " + rec.lname;
                Reciever_errorTbl.Text = "";
            }
        }
        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            Send_btn.IsEnabled = false;
            if (rec == null)
            {
                Reciever_errorTbl.Text = "Receiver is invalid";
                Send_btn.IsEnabled = true;
            }
            if (rec != null && (normalCb.IsChecked == true || importantCb.IsChecked == true || veryimportantCb.IsChecked == true))
            {
                Message m = new Message()
                {
                    topic = TopicTb.Text,
                    hw_receiver_id = rec.id,
                    hw_sender_id = user.id,
                    content = ContentTb.Text,
                    sent_time = DateTime.Today,
                    status = this.status
                };
                IMobileServiceTable<Message> msgTable = App.MobileService.GetTable<Message>();
                await msgTable.InsertAsync(m);
                
                this.Frame.Navigate(typeof(HomePage), user);
            }
            else {
                checkStatus.Visibility = Visibility.Visible;
                Send_btn.IsEnabled = true;
            }
            
        }

        private void normalCb_Checked(object sender, RoutedEventArgs e)
        {
            status = "normal";
            if (importantCb.IsChecked != null) importantCb.IsChecked = false;
            if (veryimportantCb.IsChecked != null) veryimportantCb.IsChecked = false;
        }

        private void importantCb_Checked(object sender, RoutedEventArgs e)
        {
            status = "important";
            if (normalCb.IsChecked != null) normalCb.IsChecked = false;
            if (veryimportantCb.IsChecked != null) veryimportantCb.IsChecked = false;

        }

        private void veryimportantCb_Checked(object sender, RoutedEventArgs e)
        {
            status = "very important";
            if (normalCb.IsChecked != null) normalCb.IsChecked = false;
            if (importantCb.IsChecked != null) importantCb.IsChecked = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            sendGrid.Visibility = Visibility.Collapsed;
            itemDetailTitlePanel.Visibility = Visibility.Visible;
        }

        public ToastNotification BuildToast(string content, string arg = null)
        {
            var template = ToastTemplateType.ToastText01;
            var getTemplate = ToastNotificationManager.GetTemplateContent(template);
            var getXML = getTemplate.GetElementsByTagName("text");
            var text = getTemplate.CreateTextNode(content);
            getXML[0].AppendChild(text);
            getXML = getTemplate.GetElementsByTagName("image");
            return new ToastNotification(getTemplate);
        }
        
    }

    public class MessageItem
    {
        public string topic { get; set; }
        public string content { get; set; }
        public string sender { get; set; }
        public string sender_id { get; set; }
        public DateTime sent_time { get; set; }
        public string status { get; set; }
    }

    
}
