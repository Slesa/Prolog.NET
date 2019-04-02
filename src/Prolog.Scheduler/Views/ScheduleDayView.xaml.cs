using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Prolog.Scheduler.Views
{
    public class ScheduleDayView : UserControl
    {
        public ScheduleDayView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
