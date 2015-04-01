using System;
using System.Collections.Generic;
using System.Text;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadChartContents();
        }

        private void LoadChartContents()
        {
            Random rand = new Random();
            List<NumberOfCases> NoOfCasesList = new List<NumberOfCases>();
            NoOfCasesList.Add(new NumberOfCases() { date = "2015-03-15", cases = rand.Next(0, 30) });
            NoOfCasesList.Add(new NumberOfCases() { date = "2015-03-16", cases = rand.Next(0, 30) });
            NoOfCasesList.Add(new NumberOfCases() { date = "2015-03-17", cases = rand.Next(0, 30) });
            NoOfCasesList.Add(new NumberOfCases() { date = "2015-03-18", cases = rand.Next(0, 30) });
            NoOfCasesList.Add(new NumberOfCases() { date = "2015-03-19", cases = rand.Next(0, 30) });
//            Chart NoOfCasesChart = FindChildControl<Chart>(PredictionSection, "NoOfCasesChart") as Chart;
//            (NoOfCasesChart.Series[0] as BarSeries).ItemsSource = NoOfCasesList;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
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
