using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Editor.ViewModels
{
    public class ProgramViewModel : NotificationObject
    {
        readonly IProvideProgram _programProvider;
        readonly IProvideCurrentClause _currentClauseProvider;

        public ProgramViewModel(IProvideProgram programProvider, IProvideCurrentClause currentClauseProvider)
        {
            _programProvider = programProvider;
            _currentClauseProvider = currentClauseProvider;
            _programProvider.ProgramChanged += (s, e) => Program = e.Program;
        }

        public string Title { get { return Resources.Strings.ProgramViewModel_Title; } }

        Program _program;
        public Program Program
        {
            get { return _program; }
            set
            {
                if (value == _program) return;
                _program = value;
                RaisePropertyChanged(() => Program);
            }
        }

        public Clause SelectedClause
        {
            get { return _currentClauseProvider.SelectedClause; }
            set
            {
                if (value == _currentClauseProvider.SelectedClause) return;
                _currentClauseProvider.SelectedClause = value;
                RaisePropertyChanged(() => SelectedClause);
            }
        }

        public Procedure SelectedProcedure
        {
            get { return _currentClauseProvider.SelectedProcedure; }
            set
            {
                if (value == _currentClauseProvider.SelectedProcedure) return;
                _currentClauseProvider.SelectedProcedure = value;
                RaisePropertyChanged(() => _currentClauseProvider.SelectedProcedure);
            }
        }
    }
}