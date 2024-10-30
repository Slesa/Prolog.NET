using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Prolog.Editor.ViewModels;
using Prolog.Editor.Views;

namespace Prolog.Editor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        readonly MainWindowViewModel _mainViewModel = new MainWindowViewModel();
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    // DataContext = _mainViewModel // Remember to change this line to use our private reference to the MainViewModel
                };
            
                // Listen to the ShutdownRequested-event
                // desktop.ShutdownRequested += DesktopOnShutdownRequested;
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
