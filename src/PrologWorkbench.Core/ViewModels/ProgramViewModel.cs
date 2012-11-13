using System.Windows;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prolog;
using Prolog.Code;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;

namespace PrologWorkbench.Core.ViewModels
{
    public class ProgramViewModel : NotificationObject
    {
        readonly IProvideProgram _programProvider;
        readonly IEventAggregator _eventAggregator;

        [Dependency]
        public IProvideCurrentClause CurrentClauseProvider { get; set; }

        public ProgramViewModel(IProvideProgram programProvider, IEventAggregator eventAggregator)
        {
            _programProvider = programProvider;
            _eventAggregator = eventAggregator;
            _programProvider.ProgramChanged += (s, e) => Program = e.Program;

            CopyCommand = new DelegateCommand(OnCopy, CanCopy);
            CutCommand = new DelegateCommand(OnCut, CanCut);
            PasteCommand = new DelegateCommand(OnPaste, CanPaste);
            MoveUpCommand = new DelegateCommand(OnMoveUp, CanMoveUp);
            MoveDownCommand = new DelegateCommand(OnMoveDown, CanMoveDown);

            CopyClauseCommand = new DelegateCommand(OnCopyClause);
        }

        public string Title { get { return Resources.Strings.ProgramViewModel_Title; } }

        public DelegateCommand CopyCommand { get; private set; }
        public DelegateCommand CutCommand { get; private set; }
        public DelegateCommand PasteCommand { get; private set; }
        public DelegateCommand MoveUpCommand { get; private set; }
        public DelegateCommand MoveDownCommand { get; private set; }

        public DelegateCommand CopyClauseCommand { get; private set; }

        #region Commands

        void OnCopy()
        {
            var clause = SelectedClause;
            if (clause == null) return;
            Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
            PasteCommand.RaiseCanExecuteChanged();
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
            PasteCommand.RaiseCanExecuteChanged();
        }

        bool CanCut()
        {
            return (SelectedClause != null);
        }

        void OnPaste()
        {
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return;

            var codeSentenceObject = dataObject.GetData(CodeSentenceDataObject.CodeSentenceDataFormat);
            if (codeSentenceObject == null) return;

            var codeSentence = codeSentenceObject as CodeSentence;
            // TODO Do we want to paste clauses without a head or not?
            if (codeSentence == null /*|| codeSentence.Head == null*/) return;

            _programProvider.Program.Add(codeSentence);
        }

        bool CanPaste()
        {
            // TODO Well, how to update CanExecute, when Clipboard changes?
            return true;
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return false;

            return dataObject.GetDataPresent(CodeSentenceDataObject.CodeSentenceDataFormat);
        }

        void OnMoveUp()
        {
            var clause = SelectedClause;
            if (clause == null) return;
            clause.Container.MoveUp(clause);
            MoveUpCommand.RaiseCanExecuteChanged();
            MoveDownCommand.RaiseCanExecuteChanged();
        }

        bool CanMoveUp()
        {
            return (SelectedClause != null && !SelectedClause.IsFirst);
        }

        void OnMoveDown()
        {
            var clause = SelectedClause;
            if (clause == null) return;
            clause.Container.MoveDown(clause);
            MoveUpCommand.RaiseCanExecuteChanged();
            MoveDownCommand.RaiseCanExecuteChanged();
        }

        bool CanMoveDown()
        {
            return (SelectedClause != null && !SelectedClause.IsLast);
        }

        #endregion

        void OnCopyClause()
        {
            var clause = SelectedClause;
            if (clause == null) return;

            _eventAggregator.GetEvent<SetCurrentInputEvent>().Publish(clause.CodeSentence.ToString());
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
            get { return CurrentClauseProvider.SelectedClause; }
            set
            {
                if (value == CurrentClauseProvider.SelectedClause) return;
                CurrentClauseProvider.SelectedClause = value;
                RaisePropertyChanged(() => SelectedClause);
                
                CopyCommand.RaiseCanExecuteChanged();
                CutCommand.RaiseCanExecuteChanged();
                MoveUpCommand.RaiseCanExecuteChanged();
                MoveDownCommand.RaiseCanExecuteChanged();
            }
        }

        public Procedure SelectedProcedure
        {
            get { return CurrentClauseProvider.SelectedProcedure; }
            set
            {
                if (value == CurrentClauseProvider.SelectedProcedure) return;
                CurrentClauseProvider.SelectedProcedure = value;
                RaisePropertyChanged(() => CurrentClauseProvider.SelectedProcedure);
            }
        }
    }
}