/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows;
using System.Windows.Threading;

namespace Prolog.Scheduler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppState = new AppState(this);
        }

        public new static App Current
        {
            get { return (App)(Application.Current); }
        }

        public AppState AppState { get; private set; }

        public string ApplicationTitle
        {
            get { return FindResource("ApplicationTitle") as string; }
        }

        void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        void OnApplicationExit(object sender, ExitEventArgs e)
        {
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            CommonExceptionHandlers.HandleException(null, e.Exception);
            e.Handled = true;
        }
    }
}
