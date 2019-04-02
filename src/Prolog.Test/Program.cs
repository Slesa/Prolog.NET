using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Prolog.Test
{
    class Program
    {
        static readonly string[] ProgramNames = new string[] { "list.prolog", "farmer.prolog" };

        [STAThread]
        static void Main(string[] args)
        {
            var configuration = CreateConfiguration();
            while (true)
            {
                Console.WriteLine("Options:");
                Console.WriteLine("  1 - Generate test cases");
                Console.WriteLine("  2 - Execute test cases");
                Console.WriteLine("  3 - Display grammar");
                Console.WriteLine("  X - Exit");
                Console.WriteLine();

                Console.Write("Enter option: ");
                var option = Console.ReadLine().Trim().ToUpper();

                switch (option)
                {
                    case "1":
                    {
                        Console.Write("Enter Y to confirm: ");
                        var confirm = Console.ReadLine().Trim().ToUpper();
                        if (confirm == "Y")
                        {
                            foreach (var programName in ProgramNames)
                            {
                                var programTest = new ProgramTest(programName, configuration);
                                programTest.CreateTestResults();
                            }
                        }
                    }
                        break;
                    case "2":
                    {
                        foreach (var programName in ProgramNames)
                        {
                            var programTest = new ProgramTest(programName, configuration);
                            programTest.ValidateTestResults();
                        }
                    }
                        break;
                    case "3":
                    {
                        DisplayGrammar();
                    }
                        break;
                    case "X":
                    {
                        // No action required.
                    }
                        break;
                    default:
                    {
                        Console.WriteLine("Unrecognized input {0}.", option);
                    }
                        break;
                }


                if (option == "X")
                {
                    break;
                }

                Console.WriteLine();
            }
        }

        public static void DisplayGrammar()
        {
            var sbGrammar = new StringBuilder();

            foreach (var terminalType in Prolog.Parser.Singleton.Grammar.GetTerminals())
            {
                var text = string.Format("{0} {1}", terminalType.Name, terminalType.Pattern);
                Console.WriteLine(text);
                sbGrammar.AppendLine(text);
            }

            foreach (var nonterminalType in Prolog.Parser.Singleton.Grammar.GetNonterminals())
            {
                foreach (var ruleType in nonterminalType.Rules)
                {
                    var sb = new StringBuilder();

                    sb.Append(ruleType.Lhs.Name);
                    sb.Append(" :=");

                    foreach (var terminalType in ruleType.Rhs)
                    {
                        sb.Append(" ");
                        sb.Append(terminalType.Name);
                    }

                    var text = sb.ToString();
                    Console.WriteLine(text);
                    sbGrammar.AppendLine(text);
                }
            }

            //System.Windows.Clipboard.SetText(sbGrammar.ToString());
        }

        static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
            new Dictionary<string, string>()
            {
                [$"Paths:SampleFolder"] = @"data\Samples",
                [$"Paths:TestFolder"] = @"data\Tests",
            };

        static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddIniFile("app.ini", true, true)
                .AddInMemoryCollection(DefaultConfigurationStrings)
                .Build();

            // This allows us to set a system environment variable to Development
            // when running a compiled Release build on a local workstation, so we don't
            // have to alter our real production appsettings file for compiled-local-test.
            //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            // .AddEnvironmentVariables()
            //.AddAzureKeyVault()
        }

    }
}
