#if WINDOWS_APP
    using Bing.Maps;
#endif
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls;

namespace DEDI
{
    public sealed partial class DiseaseReportPage
    {
        string latestName;
        DateTimeOffset latestDOB;
        string latestHeight;
        string latestWeight;
        bool? latestCurrentLocationR;
        bool? latestOtherR;
        string latestOther;
        string latestTel;
        bool addStatus = false;
        string latestGender;
        List<Symptom> symtoms;
        private Health_Worker user;
        DraggablePin pin;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            InitializeMap();
            symtoms = await App.MobileService.GetTable<Symptom>().ToListAsync();
            selectSymptom.ItemsSource = symtoms;
            DatePicker.MinYear = new DateTime(1985, 6, 20);
            DatePicker.MaxYear = DateTime.Today;
            NewPatient.Visibility = Visibility.Visible;
            PatientInfo.Visibility = Visibility.Collapsed;

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage), user);
        }
        
        private void SelectSymptom_Tap(object sender, SelectionChangedEventArgs e)
        {
            if (!addSymptom.Items.Contains(selectSymptom.SelectedItem))
            {
                Symptom current = selectSymptom.SelectedItem as Symptom;
                //if (current.symptom == "Diarrhea")
                //{
                //    selectSymptom.Height = 50;
                //    SymptomIntensity.Visibility = Visibility.Visible;
                //    pooBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/poo_btn.png"));
                //    pooBtn.HoverStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/poo_btn_pressed.png"));
                //    poopooBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/poopoo_btn.png"));
                //    poopooBtn.NormalStateImageSource = new BitmapImage(new Uri("ms-appx:/Assets/poopoo_btn_pressed.png"));
                //    addSymptom.Items.Add(selectSymptom.SelectedItem);
                //}
                //else
                //{
                    addSymptom.Items.Add(selectSymptom.SelectedItem);
                //}
                
            }
        }

        private void pooBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void poopooBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addSymptom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addSymptom.Items.Remove(addSymptom.SelectedItem);
        }
     
        private void AddConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            NewPatient.Visibility = Visibility.Collapsed;
            PatientInfo.Visibility = Visibility.Visible;

            int age = DateTime.Now.Year - DOBDpk.Date.Year;
            if(DOBDpk.Date.AddYears(age) > DateTime.Now) age--;

            ShowIDTbl.Text = "123456";
            ShowNameTbl.Text = NameTb.Text;
            ShowAgeTbl.Text = age.ToString("G");
            ShowHeightTbl.Text = HeightTb.Text;
            ShowWeightTbl.Text = WeightTb.Text;
            ShowTelTbl.Text = "Tel. " + TelTb.Text;

            EditNameTb.Text = NameTb.Text;
            EditDOBDpk.Date = DOBDpk.Date;
            EditHeightTb.Text = HeightTb.Text;
            EditWeightTb.Text = WeightTb.Text;
            if (CurrentLocationRBtn.IsChecked == true) EditCurrentLocationRBtn.IsChecked = true;
            else if (OtherRBtn.IsChecked == true) { OtherRBtn.IsChecked = true; EditOtherTb.Text = OtherTb.Text; }
            EditTelTb.Text = TelTb.Text;

            addStatus = true;

            AddFlyout.Hide();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            NameTb.Text = string.Empty;
            DOBDpk.Date = DateTime.Now;
            WeightTb.Text = string.Empty;
            HeightTb.Text = string.Empty;
            CurrentLocationRBtn.IsChecked = false;
            OtherRBtn.IsChecked = false;
            OtherTb.Text = string.Empty;
            TelTb.Text = string.Empty;
        }

        private void OtherRBtn_Checked(object sender, RoutedEventArgs e)
        {
            OtherTb.IsEnabled = true;
        }

        private void CurrentLocationRBtn_Checked(object sender, RoutedEventArgs e)
        {
            OtherTb.IsEnabled = false;
        }

        private void EditConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            int age = DateTime.Now.Year - EditDOBDpk.Date.Year;
            if (EditDOBDpk.Date.AddYears(age) > DateTime.Now) age--;

            ShowIDTbl.Text = "123456";
            ShowNameTbl.Text = EditNameTb.Text;
            ShowAgeTbl.Text = age.ToString("G");
            ShowHeightTbl.Text = EditHeightTb.Text;
            ShowWeightTbl.Text = EditWeightTb.Text;
            ShowTelTbl.Text = "Tel. " + EditTelTb.Text;

            EditFlyout.Hide();
        }

        private void EditCancelBtn_Click(object sender, RoutedEventArgs e)
        {
            EditNameTb.Text = latestName;
            EditDOBDpk.Date = latestDOB;
            EditHeightTb.Text = latestHeight;
            EditWeightTb.Text = latestWeight;
            EditCurrentLocationRBtn.IsChecked = latestCurrentLocationR;
            EditOtherRBtn.IsChecked = latestOtherR;
            EditOtherTb.Text = latestOther;
            EditTelTb.Text = latestTel;

            EditFlyout.Hide();
        }

        private void EditPatientInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            latestName = EditNameTb.Text;
            latestDOB = EditDOBDpk.Date;
            latestHeight = EditHeightTb.Text;
            latestWeight = EditWeightTb.Text;
            latestCurrentLocationR = EditCurrentLocationRBtn.IsChecked;
            latestOtherR = EditOtherRBtn.IsChecked;
            latestOther = EditOtherTb.Text;
            latestTel = EditTelTb.Text;
        }

        private void PatientInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationNTime.Visibility = Visibility.Collapsed;
            PatientInfoNSymptoms.Visibility = Visibility.Visible;
            Symptoms.Visibility = Visibility.Collapsed;
            if (latestGender == "M") exMaleImg.Visibility = Visibility.Visible;
            else if (latestGender == "F") exFemaleImg.Visibility = Visibility.Visible;
            inFemaleImg.Visibility = Visibility.Collapsed;
            inMaleImg.Visibility = Visibility.Collapsed;
            internalOrgan.Visibility = Visibility.Collapsed;
            ExternalBtn.Visibility = Visibility.Collapsed;
            InternalBtn.Visibility = Visibility.Collapsed;
            MaleBtn.Visibility = Visibility.Visible;
            FemaleBtn.Visibility = Visibility.Visible;
            if (addStatus==false) NewPatient.Visibility = Visibility.Visible;
            else if (addStatus==true) PatientInfo.Visibility = Visibility.Visible;
        }

        private void SymptomsBtn_Click(object sender, RoutedEventArgs e)
        {
            LocationNTime.Visibility = Visibility.Collapsed;
            NewPatient.Visibility = Visibility.Collapsed;
            PatientInfo.Visibility = Visibility.Collapsed;
            PatientInfoNSymptoms.Visibility = Visibility.Visible;
            Symptoms.Visibility = Visibility.Visible;
            MaleBtn.Visibility = Visibility.Collapsed;
            FemaleBtn.Visibility = Visibility.Collapsed;
            ExternalBtn.Visibility = Visibility.Visible;
            InternalBtn.Visibility = Visibility.Visible;
            inMaleImg.Visibility = Visibility.Collapsed;
            inFemaleImg.Visibility = Visibility.Collapsed;
            internalOrgan.Visibility = Visibility.Collapsed;
            if (latestGender == "M")
            {
                exFemaleImg.Visibility = Visibility.Collapsed;
                exMaleImg.Visibility = Visibility.Visible;
            }
            else if (latestGender == "F")
            {
                exMaleImg.Visibility = Visibility.Collapsed;
                exFemaleImg.Visibility = Visibility.Visible;
            }
        }

        private void LocationTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            PatientInfoNSymptoms.Visibility = Visibility.Collapsed;
            LocationNTime.Visibility = Visibility.Visible;
        }

        private void MaleBtn_Click(object sender, RoutedEventArgs e)
        {
            exMaleImg.Visibility = Visibility.Visible;
            exFemaleImg.Visibility = Visibility.Collapsed;
            latestGender = "M";
        }

        private void FemaleBtn_Click(object sender, RoutedEventArgs e)
        {
            exMaleImg.Visibility = Visibility.Collapsed;
            exFemaleImg.Visibility = Visibility.Visible;
            latestGender = "F";
        }

        private void ExternalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (latestGender == "M")
            {
                inMaleImg.Visibility = Visibility.Collapsed;
                internalOrgan.Visibility = Visibility.Collapsed;
                exMaleImg.Visibility = Visibility.Visible;
            }
            else if (latestGender == "F")
            {
                inFemaleImg.Visibility = Visibility.Collapsed;
                internalOrgan.Visibility = Visibility.Collapsed;
                exFemaleImg.Visibility = Visibility.Visible;
            }
        }

        private void InternalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (latestGender == "M")
            {
                exMaleImg.Visibility = Visibility.Collapsed;
                inMaleImg.Visibility = Visibility.Visible;
                internalOrgan.Visibility = Visibility.Visible;
                
            }
            else if (latestGender == "F")
            {
                exFemaleImg.Visibility = Visibility.Collapsed;
                inFemaleImg.Visibility = Visibility.Visible;
                internalOrgan.Visibility = Visibility.Visible;
            }
        }
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            string error_msg =" is required*";
            Submit.IsEnabled=false;
            if (NameTb.Text == "")
            {
                errorTbl.Text = "Patient name" + error_msg;
            }
            else if (!isTel(TelTb.Text))
            {
                errorTbl.Text = "Patient telephone" + error_msg;
            }
            else if (OtherRBtn.IsChecked == true && OtherTb.Text=="")
            {
                errorTbl.Text = "Patient location" + error_msg;
            }
            else if (OtherRBtn.IsChecked == true && OtherTb.Text == "")
            {
                errorTbl.Text = "Patient location" + error_msg;
            }
            else if(addSymptom.Items.Count==0){
                errorTbl.Text = "Symptom" + error_msg;
            }
            else
            {
                string patient_location = "";
                if (CurrentLocationRBtn.IsChecked == true)
                {
                    patient_location = AddressTB.Text;
                }
                else patient_location = OtherTb.Text;
                IMobileServiceTable<Patient> pTable = App.MobileService.GetTable<Patient>();
                Patient p = new Patient()
                {
                    name = NameTb.Text,
                    address = patient_location,
                    dob = DOBDpk.Date.UtcDateTime,
                    gender = latestGender,
                    height = Convert.ToDouble(HeightTb.Text),
                    weight = Convert.ToDouble(WeightTb.Text),
                    telephone = TelTb.Text
                };
                await pTable.InsertAsync(p);
                IMobileServiceTable<Disease_Report> dTable = App.MobileService.GetTable<Disease_Report>();
#if WINDOWS_APP
                Disease_Report d = new Disease_Report()
                {
                    description = DescriptionTb.Text,
                    hw_id = user.id,
                    longitude = Bing.Maps.MapLayer.GetPosition(pin).Longitude,
                    latitude = Bing.Maps.MapLayer.GetPosition(pin).Latitude,
                    reported_time = DateTime.Today,
                    ocurred_time = DatePicker.Date.UtcDateTime,
                    patient_id = p.id
                };
                await dTable.InsertAsync(d);
#endif
                this.Frame.Navigate(typeof(ReportsView), user);
            }
            Submit.IsEnabled = true;
        }
        public static bool isTel(string inputTel)
        {
            string strRegex = @"^\d{9,11}$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputTel))
                return (true);
            else
                return (false);
        }
        public async void InitializeMap()
        {

#if WINDOWS_APP
            myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
            myMap.ZoomLevel = 10;
            myMap.MapType = MapType.Road;


            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                          TimeSpan.FromSeconds(10));
            myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
            pin = new DraggablePin(myMap);
            //Set the location of the pin to the center of the map.
            Bing.Maps.MapLayer.SetPosition(pin, myMap.Center);

            //Set the pin as draggable.
            pin.Draggable = true;

            //Attach to the drag event of the pin.
            pin.DragEnd += Pin_Dragged;

            //Add the pin to the map.
            myMap.Children.Add(pin);
            myMap.PointerPressedOverride += myMap_PointerPressedOverride;
            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + currentPosition.Coordinate.Latitude + "," + currentPosition.Coordinate.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address;
#endif
        }

        private async void myMap_PointerPressedOverride(object sender, PointerRoutedEventArgs e)
        {
#if WINDOWS_APP
            var pointerPosition = e.GetCurrentPoint(((Map)sender));

            Bing.Maps.Location location = null;

            //Convert the point pixel to a Location coordinate
            if (((Map)sender).TryPixelToLocation(pointerPosition.Position, out location))
            {
                Bing.Maps.MapLayer.SetPosition(pin, location);
                var client = new HttpClient();
                Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + location.Latitude + "," + location.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
                var response = await client.GetAsync(Uri);
                var result = await response.Content.ReadAsStringAsync();
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
                var list = serializer.ReadObject(ms);
                RootObject jsonResponse = list as RootObject;
                AddressTB.Text = jsonResponse.results[0].formatted_address;
            }
#endif
        }




#if WINDOWS_APP
        private async void Pin_Dragged(Bing.Maps.Location obj)
        {

            var client = new HttpClient();
            Uri Uri = new Uri("https://maps.googleapis.com/maps/api/geocode/json?latlng=" + obj.Latitude + "," + obj.Longitude + "&key=AIzaSyDeJZgbdA56eyfwk660AZY0HrljWgpRtVc");
            var response = await client.GetAsync(Uri);
            var result = await response.Content.ReadAsStringAsync();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            var list = serializer.ReadObject(ms);
            RootObject jsonResponse = list as RootObject;
            AddressTB.Text = jsonResponse.results[0].formatted_address;
        }
#endif
    }
}
