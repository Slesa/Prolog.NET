using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Prolog;
using Prolog.Code;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Core.ViewModels
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

            CopyCommand = new DelegateCommand(OnCopy, CanCopy);
            CutCommand = new DelegateCommand(OnCut, CanCut);

            CopyClauseCommand = new DelegateCommand(OnCopyClause);
        }

        public string Title { get { return Resources.Strings.ProgramViewModel_Title; } }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand CutCommand { get; private set; }

        public DelegateCommand CopyClauseCommand { get; private set; }

        #region Commands

        void OnCopy()
        {
            var clause = SelectedClause;
            if (clause == null) return;
            Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
        }

        bool CanCopy()
        {
            return (SelectedClause != null);
        }

        void OnCut()
        {
            var clause = SelectedClause;
            if (clause == null) return;
            Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
            clause.Container.Remove(clause);
        }

        bool CanCut()
        {
            return (SelectedClause != null);
        }

        #endregion

        void OnCopyClause()
        {
            /*
            http://www.codeproject.com/Articles/394750/Navigating-the-different-modules-through-a-TreeVie
            var view = (ProgramView) 
            var transcriptEntry = ctrlTranscriptEntries.SelectedItem as TranscriptEntry;
            if (transcriptEntry != null)
            {
                txtCommand.Text = transcriptEntry.Text;
                txtCommand.Focus();
            }
             * */
        }

        object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(()=>SelectedItem);
                SelectedProcedure = _selectedItem as Procedure;
                SelectedClause = _selectedItem as Clause;
            }
        }

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
                
                CopyCommand.RaiseCanExecuteChanged();
                CutCommand.RaiseCanExecuteChanged();
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