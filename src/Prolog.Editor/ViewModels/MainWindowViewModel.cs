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

            var hub = MessageHub.Instance;
            hub.Subscribe<StatusMessageEvent>(x => StatusText = x.Text);
        }
        public InputViewModel InputViewModel { get; }
        public ResultViewModel ResultViewModel { get; }

        string _statusText;

        public string StatusText
        {
            get => _statusText;
            set => this.RaiseAndSetIfChanged(ref _statusText, value);
        }
    }
}
