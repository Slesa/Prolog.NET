using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Editor.ViewModels
{
    public class ProgramEditViewModel : ProgramViewModelBase
    {

        public ProgramEditViewModel(IProvideProgram programProvider, IEventAggregator eventAggregator) 
            : base(programProvider, eventAggregator)
        {
            CopyClauseCommand = new DelegateCommand(OnCopyClause);
        }

        public DelegateCommand CopyClauseCommand { get; private set; }


        void OnCopyClause()
        {
            var clause = SelectedClause;
            if (clause == null) return;

            _eventAggregator.GetEvent<SetCurrentInputEvent>().Publish(clause.CodeSentence.ToString());
        }

    }
}