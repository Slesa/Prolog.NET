/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text;

namespace PrologTest
{
    public class PrologTest
    {
        static readonly string[] ProgramNames = new string[] { "list.prolog", "farmer.prolog" };

        [STAThread]
        public static void Main(string[] args)
        {
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
                                    var programTest = new ProgramTest(programName);
                                    programTest.CreateTestResults();
                                }
                            }
                        }
                        break;
                    case "2":
                        {
                            foreach (var programName in ProgramNames)
                            {
                                var programTest = new ProgramTest(programName);
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

        public static void HelloWorld()
        {
            //CodeSentence codeSentence;

            //codeSentence = Parser.Parse("hello(world)")[0];
            //Program program = new Program();
            //program.Add(codeSentence);

            //codeSentence = Parser.Parse(":-hello(X)")[0];
            //Query query = new Query(codeSentence);

            //PrologMachine machine = PrologMachine.Create(program, query);
            //ExecutionResults results = machine.RunToSuccess();
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

            System.Windows.Clipboard.SetText(sbGrammar.ToString());
        }
    }
}
