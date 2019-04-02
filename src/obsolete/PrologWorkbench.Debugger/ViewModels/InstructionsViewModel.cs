using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class InstructionsViewModel : NotificationObject
    {
        public InstructionsViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<CurrentStackFrameChangedEvent>().Subscribe(OnStackFrameChanged);
        }

        public string Title { get { return Resources.Strings.InstructionsViewModel_Title; } }

        PrologInstructionStream _instructions;
        public PrologInstructionStream Instructions
        {
            get { return _instructions; }
            set
            {
                _instructions = value;
                RaisePropertyChanged(() => Instructions);
            }
        }

        void OnStackFrameChanged(PrologStackFrame stackFrame)
        {
            Instructions = stackFrame != null ? stackFrame.InstructionStream : null;
        }
    }
}