using Avalonia;
using Avalonia.Logging.Serilog;
using Prolog.Editor.ViewModels;
using Prolog.Editor.Views;

namespace Prolog.Editor
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
