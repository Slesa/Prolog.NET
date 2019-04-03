using Easy.MessageHub;
using ReactiveUI;

namespace Prolog.Editor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            InputViewModel = new InputViewModel();
            ResultViewModel = new ResultViewModel();
            WindowTitle = "";

            var hub = MessageHub.Instance;
            hub.Subscribe<StatusMessageEvent>(x => StatusText = x.Text);
            hub.Subscribe<FileNameChangedEvent>(x => WindowTitle = x.FileName);
        }
        public InputViewModel InputViewModel { get; }
        public ResultViewModel ResultViewModel { get; }

        string _statusText;
        public string StatusText
        {
            get => _statusText;
            set => this.RaiseAndSetIfChanged(ref _statusText, value);
        }

        string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                var title = "Prolog.Editor";
                if (!string.IsNullOrWhiteSpace(value)) title = title + " - " + value;
                this.RaiseAndSetIfChanged(ref _windowTitle, title);
            }
        }
    }
}
