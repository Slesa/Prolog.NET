using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Prolog.Editor.Views
{
    public class InputView : UserControl
    {
        public InputView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
