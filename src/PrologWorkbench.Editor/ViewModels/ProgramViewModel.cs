using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Editor.ViewModels
{
    public class ProgramViewModel : NotificationObject
    {
        public ProgramViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ProgramChangedEvent>().Subscribe(x => Program = x);
        }

        public string Title { get { return "Program"; } }

        Prolog.Program _program;
        public Prolog.Program Program
        {
            get { return _program; }
            set
            {
                if (value == _program) return;
                _program = value;
                RaisePropertyChanged(() => Program);
            }
        }

        Clause _selectedClause;
        public Clause SelectedClause
        {
            get { return _selectedClause; }
            set
            {
                if (value == _selectedClause) return;
                _selectedClause = value;
                RaisePropertyChanged(() => SelectedClause);
            }
        }

        Procedure _selectedProcedure;
        public Procedure SelectedProcedure
        {
            get { return _selectedProcedure; }
            set
            {
                if (value == _selectedProcedure) return;
                _selectedProcedure = value;
                RaisePropertyChanged(() => SelectedProcedure);
            }
        }
    }
}