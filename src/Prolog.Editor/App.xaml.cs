using Avalonia;
using Avalonia.Markup.Xaml;

namespace Prolog.Editor
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
