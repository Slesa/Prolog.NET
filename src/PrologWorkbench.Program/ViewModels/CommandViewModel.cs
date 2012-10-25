using Microsoft.Practices.Prism.Commands;

namespace PrologWorkbench.Program.ViewModels
{
    public class CommandViewModel
    {
        public CommandViewModel()
        {
            ExecuteCommand = new DelegateCommand(OnExecute);
            DebugCommand = new DelegateCommand(OnDebug);
        }

        public DelegateCommand ExecuteCommand { get; private set; }
        public DelegateCommand DebugCommand { get; private set; }

        void OnExecute()
        {


        }
        void OnDebug()
        {

        }

    }
}