using Easy.MessageHub;
using ReactiveUI;

namespace Prolog.Editor.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        public ResultViewModel()
        {
            var hub = MessageHub.Instance;
            hub.Subscribe<ResultEvent>(x => Result = x.Result);
        }

        string _result;
        public string Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }
    }
}