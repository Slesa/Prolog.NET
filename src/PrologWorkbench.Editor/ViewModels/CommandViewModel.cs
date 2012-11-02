using System.Text;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Prolog;
using PrologWorkbench.Core.Contracts;
using PrologWorkbench.Core.Events;
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
        [Dependency]
        public IProvideCurrentClause CurrentClauseProvider { get; set; }
        [Dependency]
        public IProvideStatusUpdates StatusUpdateProvider { get; set; }

        public CommandViewModel(IEventAggregator eventAggregator)
        {
            ExecuteCommand = new DelegateCommand(OnExecute, CanExecute);
            DebugCommand = new DelegateCommand(OnDebug, CanDebug);
            eventAggregator.GetEvent<SetCurrentInputEvent>().Subscribe(x => CurrentInput = x);
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

            TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Request, input);

            var selectedClause = CurrentClauseProvider.SelectedClause;

            var codeSentences = Parser.Parse(input);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.MessageUnrecognizedInput);
                return;
            }

            foreach (var codeSentence in codeSentences)
            {
                if (codeSentence.Head == null) // query
                {
                    var query = new Query(codeSentence);
                    MachineProvider.Machine = PrologMachine.Create(ProgramProvider.Program, query);
                    MachineProvider.Machine.ExecutionComplete += OnMachineExecutionComplete;
                    if (executeQuery)
                    {
                        MachineProvider.Machine.RunToSuccess();
                    }
                }
                else // fact or rule
                {
                    if (selectedClause != null && selectedClause.Container.Procedure.Functor == Functor.Create(codeSentence.Head.Functor))
                    {
                        selectedClause.CodeSentence = codeSentence;
                        TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.ResponseSuccess);
                    }
                    else
                    {
                        if (ProgramProvider.Program.Contains(codeSentence))
                        {
                            TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.MessageDuplicateClause);
                        }
                        else
                        {
                            ProgramProvider.Program.Add(codeSentence);
                            TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.ResponseSuccess);
                        }
                    }
                }
            }
        }


        void OnMachineExecutionComplete(object sender, PrologQueryEventArgs e)
        {
            if (e.Results != null)
            {
                var sb = new StringBuilder();

                string prefix = null;
                foreach (var variable in e.Results.Variables)
                {
                    sb.Append(prefix); prefix = System.Environment.NewLine;
                    sb.AppendFormat("{0} = {1}", variable.Name, variable.Text);
                }

                var variables = sb.ToString();
                if (!string.IsNullOrEmpty(variables))
                {
                    TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, variables);
                }
                TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.ResponseSuccess);

                //if (StatisticsEnabled)
                {
                    //TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, string.Format(Resources.Strings.CommandViewModel_ExecutionTime
                    StatusUpdateProvider.Publish(string.Format(Resources.Strings.CommandViewModel_ExecutionTime
                        , MachineProvider.Machine.PerformanceStatistics.ElapsedTime
                        , MachineProvider.Machine.PerformanceStatistics.InstructionCount));
                }
            }
            else
            {
                TranscriptProvider.Transcript.AddTranscriptEntry(TranscriptEntryTypes.Response, Resources.Strings.ResponseFailure);
            }
        }
    }
}