using Microsoft.Practices.Prism.ViewModel;
using Prolog;

namespace PrologWorkbench.Program.ViewModels
{
    public class ProgramTreeViewModel : NotificationObject
    {
        Procedure _selectedProcedure;
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