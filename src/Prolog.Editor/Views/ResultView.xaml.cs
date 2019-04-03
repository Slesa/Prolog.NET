using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Prolog.Editor.Views
{
    public class ResultView : UserControl
    {
        public ResultView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
