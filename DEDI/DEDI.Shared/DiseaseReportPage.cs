using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NewPatient.Visibility = Visibility.Visible;
            PatientInfo.Visibility = Visibility.Collapsed;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreateReportPage));
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
                exMaleImg.Visibility = Visibility.Visible;
            }
            else if (latestGender == "F")
            {
                inFemaleImg.Visibility = Visibility.Collapsed;
                exFemaleImg.Visibility = Visibility.Visible;
            }
        }

        private void InternalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (latestGender == "M")
            {
                exMaleImg.Visibility = Visibility.Collapsed;
                inMaleImg.Visibility = Visibility.Visible;
            }
            else if (latestGender == "F")
            {
                exFemaleImg.Visibility = Visibility.Collapsed;
                inFemaleImg.Visibility = Visibility.Visible;
            }
        }
    }
}
