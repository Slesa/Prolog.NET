using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Program.ViewModels
{
    public class StatusBarViewModel : NotificationObject
    {
        string _message;

        public StatusBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<StatusUpdateEvent>().Subscribe(x => Message = x);
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value == Message) return;
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }
    }
}