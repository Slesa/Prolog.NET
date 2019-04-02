using Avalonia;
using Avalonia.Logging.Serilog;
using Prolog.Scheduler.ViewModels;
using Prolog.Scheduler.Views;

namespace Prolog.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
