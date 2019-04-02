using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using Prolog.Scheduler.Models;

namespace Prolog.Scheduler
{
    public class Scheduler
    {
        readonly IConfiguration _configuration;
        const string SampleFile = "schedule.prolog";

        public Scheduler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        Prolog.Program _program;
        public Prolog.Program Program
        {
            get
            {
                if (_program == null)
                {
                    var fileName = Path.Combine(_configuration["Paths:DataFolder"], SampleFile);
                    if (!File.Exists(fileName))
                        fileName = AskFileName().Result;
                    if (!File.Exists(fileName))
                    {
                        throw new FileNotFoundException(
                            $"{fileName} not found.  Consider updating SamplesFolder setting during program development.");
                    }
                    _program = CreateProgram(fileName).Result;
                }
                return _program;
            }
        }

        async Task<Prolog.Program> CreateProgram(string fileName)
        {
            var result = await Task.Run(() => Prolog.Program.Load(fileName));
            return result;
        }

        async Task<string> AskFileName()
        {
            var dialog = new OpenFileDialog()
            {
                InitialFileName = SampleFile,
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

        Query _query;
        public Query Query
        {
            get
            {
                if (_query == null)
                {
                    var codeSentenceQuery = Parser.Parse(":- solve(X)")[0];
                    var query = new Query(codeSentenceQuery);
                    _query = query;
                }
                return _query;
            }
        }

        PrologMachine _machine;
        public PrologMachine Machine => _machine ?? (_machine = PrologMachine.Create(Program, Query));

        public void Restart()
        {
            var machine = Machine;
            machine.Restart();
        }

        public Schedule Execute()
        {
            var machine = Machine;
            if (!machine.CanRunToSuccess)
            {
                return null;
            }

            var executionResults = machine.RunToSuccess();
            if (executionResults != ExecutionResults.Success)
            {
                return null;
            }

            var codeTerm = machine.QueryResults.Variables[0].CodeTerm;
            var schedule = Schedule.Create(codeTerm);
            return schedule;
        }
    }
}