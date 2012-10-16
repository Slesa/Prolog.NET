/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
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

        void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            GetNextSolution();
        }

        void OnMnuFileExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void OnBtnNextSolutionClick(object sender, RoutedEventArgs e)
        {
            GetNextSolution();
        }

        void OnBtnRestartClick(object sender, RoutedEventArgs e)
        {
            App.Current.AppState.Scheduler.Restart();
            GetNextSolution();
        }

        void GetNextSolution()
        {
            var schedule = App.Current.AppState.Scheduler.Execute();
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
