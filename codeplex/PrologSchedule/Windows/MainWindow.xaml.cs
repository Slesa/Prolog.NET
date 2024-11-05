/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Prolog.Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = App.Current.AppState;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetNextSolution();
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnNextSolution_Click(object sender, RoutedEventArgs e)
        {
            GetNextSolution();
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            App.Current.AppState.Scheduler.Restart();

            GetNextSolution();
        }

        private void GetNextSolution()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Schedule schedule = App.Current.AppState.Scheduler.Execute();
            stopWatch.Stop();
            var ts = stopWatch.Elapsed;
            var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:00}";
            System.Diagnostics.Debug.WriteLine($"Calculation took {elapsedTime} - Ready");
            
            if (schedule == null)
            {
                MessageBox.Show("No more schedules exist.", "Scheduler", MessageBoxButton.OK);
            }
            else
            {
                App.Current.AppState.Schedule = schedule;
            }
        }
    }
}
