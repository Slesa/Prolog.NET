using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
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
                    (hasCode) => !string.IsNullOrWhiteSpace(hasCode.Value)));
            //.Subscribe(_ => OnExecute());
            //OkButton = ReactiveCommand.Create(
            //    this.WhenAny(x => x.Code, y => y.Query,
            //        (x, y) => !string.IsNullOrWhiteSpace(x.Value) && !string.IsNullOrWhiteSpace(y.Value)) );
            // this.WhenAny(x => x.Code, y=> y.Query, (x, y) => !string.IsNullOrWhiteSpace(x) && !string.IsNullOrWhiteSpace(y));
            LoadCommand = ReactiveCommand.Create(OnLoad);
            SaveCommand = ReactiveCommand.Create(OnSave);
        }

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

        public ReactiveCommand<Unit, Unit> LoadCommand { get; }
        void OnLoad()
        {
            var fileName = AskFileName().Result;
            if (fileName == null || !File.Exists(fileName)) return;
            var text = File.ReadAllLines(fileName);
            var code = "";
            foreach (var line in text)
            {
                var c = line.Trim();
                if (c.StartsWith(":-"))
                {
                    Query = c;
                    continue;
                }
                code += line;
            }
            Code = code;
        }

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        void OnSave()
        {

        }

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }
        void OnExecute()
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
            PrologMachine machine = null;
            try
            {
                machine = PrologMachine.Create(program, query);
                machine.ExecutionComplete += CodeExecuted;
                var result = machine.RunToSuccess();

                var execResult = Enum.GetName(typeof(ExecutionResults), result);
                _hub.Publish(new StatusMessageEvent($"Result of execution: {execResult}"));
            }
            catch (Exception ex)
            {
                if(machine!=null) machine.ExecutionComplete -= CodeExecuted;
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

        async Task<string> AskFileName()
        {
            var dialog = new OpenFileDialog()
            {
                //InitialFileName = SampleFile,
                Filters = new List<FileDialogFilter>
                { new FileDialogFilter
                {
                    Extensions = new List<string> { "*.prolog", "*.plg"},
                    Name = "Prolog Files"
                } },
                Title = "Could not locate sample file",
                InitialDirectory = Directory.GetCurrentDirectory()
            };
            var result = await dialog.ShowAsync();
            return result.FirstOrDefault();
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
            var buffer = Query.Trim();
            if (!buffer.StartsWith(":-"))
                buffer = ":-" + buffer;
            var codeSentences = Parser.Parse(buffer);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                _hub.Publish(new StatusMessageEvent("Query sentence was empty"));
                return null;
            }
            return new Query(codeSentences[0]);
        }
    }
}