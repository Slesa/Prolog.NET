using Avalonia;
using Avalonia.Markup.Xaml;

namespace Prolog.Scheduler
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
