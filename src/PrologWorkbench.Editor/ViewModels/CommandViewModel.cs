using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prolog;
using PrologWorkbench.Core.Contracts;

namespace PrologWorkbench.Editor.ViewModels
{
    public class CommandViewModel : NotificationObject
    {
        string _currentInput;

        [Dependency]
        public IProvideMachine MachineProvider { get; set; }
        [Dependency]
        public IProvideProgram ProgramProvider { get; set; }

        public CommandViewModel()
        {
            ExecuteCommand = new DelegateCommand(OnExecute);
            DebugCommand = new DelegateCommand(OnDebug);
        }

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
            }
        }

        void OnExecute()
        {
            ProcessInput(true);
        }

        void OnDebug()
        {
            ProcessInput(false);
        }

        void ProcessInput(bool executeQuery)
        {
            var input = CurrentInput;
            if (input == null) return;

            input = input.Trim();
            if (string.IsNullOrEmpty(input)) return;


            /*
            AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Request, input);

            var selectedClause = ctrlProgram.SelectedClause;
            */

            var codeSentences = Parser.Parse(input);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                //AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.MessageUnrecognizedInput);
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
                        if (MachineProvider.Machine.Program.Contains(codeSentence))
                        {
                            //AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.MessageDuplicateClause);
                        }
                        else
                        {
                            MachineProvider.Machine.Program.Add(codeSentence);
                            //AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseSuccess);
                        }
                    }
                }
            }
        }
    }
}