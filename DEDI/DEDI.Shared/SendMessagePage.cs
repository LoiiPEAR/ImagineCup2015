using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class SendMessagePage
    {
        Health_Worker user;
        Health_Worker rec;
        List<Health_Worker> contact;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           
            user = e.Parameter as Health_Worker;
            

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
            Send_btn.IsEnabled=false;
            if (rec == null)
            {
                Reciever_errorTbl.Text = "Receiver is invalid";
            }
            if(rec!=null){
                Message m = new Message()
                {
                    topic = TopicTb.Text,
                    hw_receiver_id = rec.id,
                    hw_sender_id = user.id,
                    content = ContentTb.Text,
                    sent_time = DateTime.Today
                };
                IMobileServiceTable<Message> msgTable = App.MobileService.GetTable<Message>();
                await msgTable.InsertAsync(m);
                this.Frame.Navigate(typeof(HomePage), user);
            }
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
}
