using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Easy.MessageHub;
using Prolog.Code;
using ReactiveUI;

namespace Prolog.Editor.ViewModels
{
    public class InputViewModel : ViewModelBase
    {
        readonly MessageHub _hub;

        public InputViewModel()
        {
            _hub = MessageHub.Instance;
            //ExecuteCommand = ReactiveCommand.Create(OnExecute);
            ExecuteCommand = ReactiveCommand.Create(OnExecute,
                this.WhenAny(
                    x => x.Code,
                    hasCode => !string.IsNullOrWhiteSpace(hasCode.Value)));
            //.Subscribe(_ => OnExecute());
            //OkButton = ReactiveCommand.Create(
            //    this.WhenAny(x => x.Code, y => y.Query,
            //        (x, y) => !string.IsNullOrWhiteSpace(x.Value) && !string.IsNullOrWhiteSpace(y.Value)) );
            // this.WhenAny(x => x.Code, y=> y.Query, (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y));
            LoadCommand = ReactiveCommand.CreateFromObservable(OnLoad);
            LoadCommand.Subscribe(ReadFile);
            SaveCommand = ReactiveCommand.Create(OnSave,
                this.WhenAny(
                    x => x.FileName,
                    fileName => !string.IsNullOrWhiteSpace(fileName.Value)));
        }

        public PrologMachine Machine { get; private set; }
        public ReactiveCommand<Unit, string> LoadCommand { get; }

        string _code;
        public string Code
        {
            get => _code;
            set => this.RaiseAndSetIfChanged(ref _code, value);
        }

        string _query;
        public string Query
        {
            get => _query;
            set => this.RaiseAndSetIfChanged(ref _query, value);
        }

        string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _hub.Publish(new FileNameChangedEvent(value));
                this.RaiseAndSetIfChanged(ref _fileName, value);
            }
        }


        IObservable<string> OnLoad()
        {
            return Observable.Create<string>(async (observer, token) =>
            {
                var files = await AskFileName();
                if (files != null) observer.OnNext(files[0]);
                observer.OnCompleted();
            });
        }

        void ReadFile(string fileName)
        {
            if (fileName == null || !File.Exists(fileName)) return;
            var text = File.ReadAllLines(fileName);
            var code = "";
            var query = "";
            foreach (var line in text)
            {
                var c = line.Trim();
                if (c.StartsWith(":-"))
                {
                    query = c;
                    continue;
                }
                code += line + Environment.NewLine;
            }
            Query = query;
            Code = code;
            FileName = fileName;
            Machine = null;
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        void OnSave()
        {
            var sb = new StringBuilder();
                sb.AppendLine(Code)
                    .AppendLine("")
                    .AppendLine(GetQueryText());
            File.WriteAllText(FileName, sb.ToString());
        }

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        void OnExecute()
        {
            if (Machine == null)
            {
                var codeSentences = GetCodeSentences();
                if (codeSentences == null)
                    return;
                var query = GetQuery();
                if (query == null)
                    return;
                var program = new Prolog.Program();
                foreach (var sentence in codeSentences)
                    program.Add(sentence);
                _hub.Publish(new ResultEvent(""));
                try
                {
                    Machine = PrologMachine.Create(program, query);
                }
                catch (Exception ex)
                {
                    _hub.Publish(new StatusMessageEvent($"Unable to create environment: {ex.Message}"));
                }
            }
            ExecuteNext();
        }

        void ExecuteNext()
        {
            try
            {
                Machine.ExecutionComplete += CodeExecuted;
                var result = Machine.RunToSuccess();

                var execResult = Enum.GetName(typeof(ExecutionResults), result);
                _hub.Publish(new StatusMessageEvent($"Result of execution: {execResult}"));
            }
            catch (Exception ex)
            {
                if (Machine != null) Machine.ExecutionComplete -= CodeExecuted;
                _hub.Publish(new StatusMessageEvent($"Got error in execution: {ex.Message}"));
            }
        }

        void CodeExecuted(object sender, PrologQueryEventArgs e)
        {
            var machine = sender as PrologMachine;
            if (machine == null) return;
            machine.ExecutionComplete -= CodeExecuted;

            if (e.Results == null)
            {
                _hub.Publish(new StatusMessageEvent("No result available"));
                return;
            }

            var sb = new StringBuilder();

            string prefix = null;
            foreach (var variable in e.Results.Variables)
            {
                sb.Append(prefix); prefix = Environment.NewLine;
                sb.AppendFormat("{0} = {1}", variable.Name, variable.Text);
            }

            _hub.Publish(new ResultEvent(sb.ToString()));
        }

        Task<string[]> AskFileName()
        {
            var dialog = new OpenFileDialog()
            {
                //InitialFileName = SampleFile,
                Filters = new List<FileDialogFilter>
                { new FileDialogFilter {
                    Extensions = new List<string> { "prolog", "pl"},
                    Name = "Prolog Files" },
                  new FileDialogFilter {
                    Extensions = new List<string> { "txt"},
                    Name = "Text Files" },
                  new FileDialogFilter {
                    Extensions = new List<string> { "*"},
                    Name = "All Files" },
                },
                Title = "Could not locate sample file",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            return dialog.ShowAsync();
        }

        IEnumerable<CodeSentence> GetCodeSentences()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                _hub.Publish(new StatusMessageEvent("Code was empty"));
                return null;
            }
            var codeSentences = Prolog.Parser.Parse(Code);
            if (codeSentences == null)
            {
                _hub.Publish(new StatusMessageEvent("Code sentences were empty"));
                return null;
            }
            return codeSentences;
        }

        Query GetQuery()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                _hub.Publish(new StatusMessageEvent("Query was empty"));
                return null;
            }

            var query = GetQueryText();
            var codeSentences = Parser.Parse(query);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                _hub.Publish(new StatusMessageEvent("Query sentence was empty"));
                return null;
            }
            return new Query(codeSentences[0]);
        }

        string GetQueryText()
        {
            var result = Query.Trim();
            if (!result.StartsWith(":-"))
                result = ":-" + result;
            return result;
        }
    }
}