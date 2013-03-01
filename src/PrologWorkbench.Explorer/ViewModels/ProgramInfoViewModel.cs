using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Explorer.ViewModels
{
    public class ProgramInfoViewModel : ProgramViewModelBase
    {
        public ProgramInfoViewModel(IProvideProgram programProvider, IEventAggregator eventAggregator) 
            : base(programProvider, eventAggregator)
        {
            ShowInstructionsCommand = new DelegateCommand(OnShowInstructions);
        }

        public DelegateCommand ShowInstructionsCommand { get; private set; }

        void OnShowInstructions()
        {
            var clause = SelectedClause;
            if (clause == null) return;

            //_eventAggregator.GetEvent<SetCurrentInputEvent>().Publish(clause.CodeSentence.ToString());
        }
    }
}