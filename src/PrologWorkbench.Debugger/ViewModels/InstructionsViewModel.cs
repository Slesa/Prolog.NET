using Microsoft.Practices.Prism.ViewModel;
using Prolog;

namespace PrologWorkbench.Debugger.ViewModels
{
    public class InstructionsViewModel : NotificationObject
    {
        public string Title { get { return Resources.Strings.InstructionsViewModel_Title; } }

        PrologStackFrame _stackFrame;
        public PrologStackFrame StackFrame
        {
            get { return _stackFrame; }
            set
            {
                _stackFrame = value;
                RaisePropertyChanged(() => StackFrame);
            }
        }
    }
}