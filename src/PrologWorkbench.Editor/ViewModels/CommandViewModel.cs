using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prolog;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Models;

namespace PrologWorkbench.Editor.ViewModels
{
    public class CommandViewModel : NotificationObject
    {
        string _currentInput;

        [Dependency]
        public IProvideMachine MachineProvider { get; set; }
        [Dependency]
        public IProvideProgram ProgramProvider { get; set; }
        [Dependency]
        public IProvideTranscript TranscriptProvider { get; set; }

        public CommandViewModel()
        {
            ExecuteCommand = new DelegateCommand(OnExecute, CanExecute);
            DebugCommand = new DelegateCommand(OnDebug, CanDebug);
        }

        public string Title { get { return Resources.Strings.CommandViewModel_Title; } }

        public DelegateCommand ExecuteCommand { get; private set; }
        public DelegateCommand DebugCommand { get; private set; }

        public string CurrentInput
        {
            get { return _currentInput; }
            set
            {
                if (value == _currentInput) return;
                _currentInput = value;
                RaisePropertyChanged(()=>CurrentInput);
                ExecuteCommand.RaiseCanExecuteChanged();
                DebugCommand.RaiseCanExecuteChanged();
            }
        }

        #region Commands

        bool CanExecute()
        {
            return !string.IsNullOrEmpty(CurrentInput);
        }

        void OnExecute()
        {
            ProcessInput(true);
        }

        bool CanDebug()
        {
            return !string.IsNullOrEmpty(CurrentInput);
        }

        void OnDebug()
        {
            ProcessInput(false);
        }

        #endregion

        void ProcessInput(bool executeQuery)
        {
            var input = CurrentInput;
            if (input == null) return;

            input = input.Trim();
            if (string.IsNullOrEmpty(input)) return;

            TranscriptProvider.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Request, input);

            /*
            var selectedClause = ctrlProgram.SelectedClause;
            */

            var codeSentences = Parser.Parse(input);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                TranscriptProvider.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.MessageUnrecognizedInput);
                return;
            }

            foreach (var codeSentence in codeSentences)
            {
                if (codeSentence.Head == null) // query
                {
                    var query = new Query(codeSentence);
                    MachineProvider.Machine = PrologMachine.Create(ProgramProvider.Program, query);

                    if (executeQuery)
                    {
                        MachineProvider.Machine.RunToSuccess();
                    }
                }
                else // fact or rule
                {
                    /*
                    if (selectedClause != null && selectedClause.Container.Procedure.Functor == Functor.Create(codeSentence.Head.Functor))
                    {
                        selectedClause.CodeSentence = codeSentence;
                        AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseSuccess);
                    }
                    else*/
                    {
                        if (ProgramProvider.Program.Contains(codeSentence))
                        {
                            TranscriptProvider.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.MessageDuplicateClause);
                        }
                        else
                        {
                            ProgramProvider.Program.Add(codeSentence);
                            TranscriptProvider.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.ResponseSuccess);
                        }
                    }
                }
            }
        }
    }
}