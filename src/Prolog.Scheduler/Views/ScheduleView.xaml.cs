using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Prolog.Scheduler.Views
{
    public class ScheduleView : UserControl
    {
        public ScheduleView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
