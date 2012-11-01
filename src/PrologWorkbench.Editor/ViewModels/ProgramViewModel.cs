using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Editor.ViewModels
{
    public class ProgramViewModel : NotificationObject
    {
        readonly IProvideProgram _programProvider;

        public ProgramViewModel(IProvideProgram programProvider)
        {
            _programProvider = programProvider;
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