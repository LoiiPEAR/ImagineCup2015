using Bing.Maps;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace DEDI
{
    public sealed partial class DashBoard
    {
        public class NumberOfCases
        {
            public string date { get; set; }
            public int cases { get; set; }
        }
        Health_Worker user;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as Health_Worker;
            loaddata();
        }

        private async void loaddata()
        {
            try
            {
                Geolocator geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                Geoposition currentPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1),
                                                                               TimeSpan.FromSeconds(10));
                Map myMap = FindChildControl<Map>(ResponsibleAreaSection, "myMap") as Map;
                myMap.Credentials = "AoLBvVSHDImAEcL4sNj6pWaEUMNR-lOCm_D_NtXhokvHCMOoKI7EnpJ_9A8dH5Ht";
                myMap.ZoomLevel = 10;
                myMap.MapType = MapType.Road;
                myMap.Center = new Bing.Maps.Location(currentPosition.Coordinate.Latitude, currentPosition.Coordinate.Longitude);
                loadgraph();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void loadgraph()
        {
            Random rand = new Random();
            List<NumberOfCases> All = new List<NumberOfCases>();
            List<NumberOfCases> Cholera = new List<NumberOfCases>();
            List<NumberOfCases> Rotavirus = new List<NumberOfCases>();
            List<NumberOfCases> Shigella = new List<NumberOfCases>();
            List<NumberOfCases> Typhoid = new List<NumberOfCases>();
            List<NumberOfCases> Others = new List<NumberOfCases>();
            All.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            All.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Cholera.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Rotavirus.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Shigella.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Typhoid.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            Others.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
            Chart NoOfCasesChart = FindChildControl<Chart>(PredictionSection, "NoOfCasesChart") as Chart;
            (NoOfCasesChart.Series[0] as ColumnSeries).ItemsSource = All;
            (NoOfCasesChart.Series[1] as ColumnSeries).ItemsSource = Cholera;
            (NoOfCasesChart.Series[2] as ColumnSeries).ItemsSource = Rotavirus;
            (NoOfCasesChart.Series[3] as ColumnSeries).ItemsSource = Shigella;
            (NoOfCasesChart.Series[4] as ColumnSeries).ItemsSource = Typhoid;
            (NoOfCasesChart.Series[5] as ColumnSeries).ItemsSource = Others;
            Button NoOfCasesBtn = FindChildControl<Button>(PredictionSection, "NoOfCasesBtn") as Button;
            NoOfCasesBtn.Visibility = Visibility.Collapsed;
        }

        private void NoOfCasesBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage),user);
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid ProbReport = FindChildControl<Grid>(PredictionSection, "ProbReport") as Grid;
            ProbReport.Height = 250;
            Button MoreBtn = FindChildControl<Button>(PredictionSection, "MoreBtn") as Button;
            MoreBtn.Visibility = Visibility.Collapsed;
            Button LessBtn = FindChildControl<Button>(PredictionSection, "LessBtn") as Button;
            LessBtn.Visibility = Visibility.Visible;
        }

        private void LessBtn_Click(object sender, RoutedEventArgs e)
        {
            Grid ProbReport = FindChildControl<Grid>(PredictionSection, "ProbReport") as Grid;
            ProbReport.Height = 130;
            Button MoreBtn = FindChildControl<Button>(PredictionSection, "MoreBtn") as Button;
            MoreBtn.Visibility = Visibility.Visible;
            Button LessBtn = FindChildControl<Button>(PredictionSection, "LessBtn") as Button;
            LessBtn.Visibility = Visibility.Collapsed;
        }

        private DependencyObject FindChildControl<T>(DependencyObject control, string ctrlName)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == ctrlName)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = FindChildControl<T>(child, ctrlName);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

    }
}
