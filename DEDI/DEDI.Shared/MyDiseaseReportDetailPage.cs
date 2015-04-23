﻿using Bing.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DEDI
{
    public sealed partial class MyDiseaseReportDetailPage
    {
        Health_Worker user;

        Disease_Report_View report;
        DiseaseReportDetail reportdetail;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            reportdetail = e.Parameter as DiseaseReportDetail;
            loaddata();
            InitializeMap();
        }
        private async void loaddata()
        {
            user = reportdetail.hw;
            report = reportdetail.report;
            DescriptionTb.Text = report.description;
            DateTbl.Text = report.ocurred_time + "";

            var hw = await App.MobileService.GetTable<Health_Worker>().Where(r => r.id == report.hw_id).ToListAsync();
            //NameTb.Text = hw[0].fname + " " + hw[0].lname;
            var patient = await App.MobileService.GetTable<Patient>().Where(r => r.id == report.patient_id).ToListAsync();
            ShowHeightTbl.Text = patient[0].height + " cm.";
            ShowNameTbl.Text = patient[0].name;
            ShowTelTbl.Text = patient[0].telephone;
            ShowWeightTbl.Text = patient[0].weight + " kg.";
            int age = DateTime.Now.Year - patient[0].dob.Date.Year;
            if (patient[0].dob.Date.AddYears(age) > DateTime.Now) age--;
            ShowAgeTbl.Text = age + "";
            if (patient[0].gender != null)
            {
                if (patient[0].gender.Equals("F")) AvatarImg.Source = new BitmapImage(new Uri("ms-appx:/Assets/female-external.png"));
                else AvatarImg.Source = new BitmapImage(new Uri("ms-appx:/Assets/male-external.png"));

            }
            else AvatarImg.Source = new BitmapImage(new Uri("ms-appx:/Assets/male-external.png"));


            List<Reported_Symptom> s = new List<Reported_Symptom>();
            var symptoms = await App.MobileService.GetTable<Reported_Symptom>().Where(r => r.disease_report_id == report.id).ToListAsync();
            foreach (Reported_Symptom symptom in symptoms)
            {

                if (symptom.symptom == "stool_frequncy_per_day")
                {
                    if (symptom.intensity == "mt 4")
                    {
                        pooImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/poopoo_btn.png"));
                        pooTbl.Text = "&gt; 4 times/day";
                    }
                    else if (symptom.intensity == "lt 4")
                    {
                        pooImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/poo_btn.png"));
                        pooTbl.Text = "&lt; 4 times/day";
                    }

                }
                else if (symptom.symptom == "nature_of_stool")
                {
                    if (symptom.intensity == "watery")
                    {
                        stoolNatureTbl.Text = "Watery";
                    }
                    else if (symptom.intensity == "loose")
                    {
                        stoolNatureTbl.Text = "Loose";
                    }
                }
                else if (symptom.symptom == "stool_type")
                {
                    if (symptom.intensity == "normal")
                    {
                        stoolTypeTbl.Text = "normal";
                    }
                    else if (symptom.intensity == "mucus")
                    {
                        stoolTypeTbl.Text = "mucus";
                    }
                    else if (symptom.intensity == "blood")
                    {
                        stoolTypeTbl.Text = "blood";
                    }
                }
                else if (symptom.symptom == "urine_output")
                {
                    if (symptom.intensity == "llt 1ml kg hr")
                    {
                        urineImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/reduced_urine_btn.png"));
                        urineTbl.Text = ">> 1 ml/kg/hr";
                    }
                    else if (symptom.intensity == "lt 1ml kg hr")
                    {
                        urineImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/urine_btn.png"));
                        urineTbl.Text = "> 1 ml/kg/hr";
                    }
                    s.Add(symptom);

                }
                else if (symptom.symptom == "decreased_skin_turgor")
                {
                    if (symptom.intensity == "delay mt 5s")
                    {
                        skinTurgorImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/deskin_btn.png"));
                        skinTurgorTbl.Text = "Delay > 5 sec";
                    }
                    else if (symptom.intensity == "delay 2 5s")
                    {
                        skinTurgorImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/skin_btn.png"));
                        skinTurgorTbl.Text = "Delay 2-5 sec";
                    }
                    s.Add(symptom);

                }
                else if (symptom.symptom == "thirst")
                {
                    if (symptom.intensity == "thirsty")
                    {
                        thirstyImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/thirst_btn.png"));
                        thirstyTbl.Text = "Thirsty";
                    }
                    else if (symptom.intensity == "unable to drink")
                    {
                        thirstyImage.Source = new BitmapImage(new Uri("ms-appx:/Assets/unable_btn.png"));
                        thirstyTbl.Text = "Unable to drink";
                    }
                    s.Add(symptom);

                }
                else s.Add(symptom);
            }
            SymptomLv.ItemsSource = s;
        }
        private void SelectSymptom_Tap(object sender, SelectionChangedEventArgs e)
        {

            Reported_Symptom current = SymptomLv.SelectedItem as Reported_Symptom;
            if (current.symptom == "Diarrhea")
            {

                DiarrheaIntensity.Visibility = Visibility.Visible;
                urineOutput.Visibility = Visibility.Collapsed;
                skinTurgor.Visibility = Visibility.Collapsed;
                thirst.Visibility = Visibility.Collapsed;
            }
            else if (current.symptom == "Decreased skin turgor")
            {

                DiarrheaIntensity.Visibility = Visibility.Collapsed;
                urineOutput.Visibility = Visibility.Collapsed;
                skinTurgor.Visibility = Visibility.Visible;
                thirst.Visibility = Visibility.Collapsed;
            }
            else if (current.symptom == "Reduced urine")
            {

                DiarrheaIntensity.Visibility = Visibility.Collapsed;
                urineOutput.Visibility = Visibility.Visible;
                skinTurgor.Visibility = Visibility.Collapsed;
                thirst.Visibility = Visibility.Collapsed;
            }
            else if (current.symptom == "Thirst")
            {

                DiarrheaIntensity.Visibility = Visibility.Collapsed;
                urineOutput.Visibility = Visibility.Collapsed;
                skinTurgor.Visibility = Visibility.Collapsed;
                thirst.Visibility = Visibility.Visible;
            }
            else
            {
                DiarrheaIntensity.Visibility = Visibility.Collapsed;
                urineOutput.Visibility = Visibility.Collapsed;
                skinTurgor.Visibility = Visibility.Collapsed;
                thirst.Visibility = Visibility.Collapsed;
            }


        }
        private async void InitializeMap()
        {
#if WINDOWS_APP
            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 17;
            myMap.MapType = MapType.Road;

            Pushpin pushpin = new Pushpin();
            pushpin.Background = new SolidColorBrush(Colors.Blue);
            MapLayer.SetPosition(pushpin, new Bing.Maps.Location(report.latitude, report.longitude));
            myMap.Children.Add(pushpin);
            myMap.Center = new Bing.Maps.Location(report.latitude, report.longitude);


            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + report.latitude + "," + report.longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address;
#endif
        }
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyReportPage), user);
        }

        private void PatientInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationNTime.Visibility = Visibility.Collapsed;
            PatientInfoNSymptoms.Visibility = Visibility.Visible;
            Symptoms.Visibility = Visibility.Collapsed;
        }

        private void SymptomsBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationNTime.Visibility = Visibility.Collapsed;
            PatientInfo.Visibility = Visibility.Collapsed;
            PatientInfoNSymptoms.Visibility = Visibility.Visible;
            Symptoms.Visibility = Visibility.Visible;

        }

        private void LocationTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            PatientInfoNSymptoms.Visibility = Visibility.Collapsed;
            LocationNTime.Visibility = Visibility.Visible;
        }
    }
    

}