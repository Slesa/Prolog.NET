using Easy.MessageHub;
using ReactiveUI;

namespace Prolog.Editor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        readonly MessageHub _hub;
        
        public MainWindowViewModel()
        {
            _hub = new MessageHub();
            InputViewModel = new InputViewModel(_hub);
            ResultViewModel = new ResultViewModel(_hub);
            WindowTitle = "";

            _hub.Subscribe<StatusMessageEvent>(x => StatusText = x.Text);
            _hub.Subscribe<FileNameChangedEvent>(x => WindowTitle = x.FileName);
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
