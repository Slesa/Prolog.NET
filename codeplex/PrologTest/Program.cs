/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text;

using Lingua;
using Prolog;
using Prolog.Code;

namespace PrologTest
{
    public class PrologTest
    {
        private static string[] ProgramNames = new string[] { "list.prolog", "farmer.prolog" };

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
                string option = Console.ReadLine().Trim().ToUpper();

                switch (option)
                {
                    case "1":
                        {
                            Console.Write("Enter Y to confirm: ");
                            string confirm = Console.ReadLine().Trim().ToUpper();
                            if (confirm == "Y")
                            {
                                foreach (string programName in ProgramNames)
                                {
                                    ProgramTest programTest = new ProgramTest(programName);
                                    programTest.CreateTestResults();
                                }
                            }
                        }
                        break;

                    case "2":
                        {
                            foreach (string programName in ProgramNames)
                            {
                                ProgramTest programTest = new ProgramTest(programName);
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
            StringBuilder sbGrammar = new StringBuilder();

            foreach (TerminalType terminalType in Prolog.Parser.Singleton.Grammar.GetTerminals())
            {
                string text = string.Format("{0} {1}", terminalType.Name, terminalType.Pattern);
                Console.WriteLine(text);
                sbGrammar.AppendLine(text);
            }

            foreach (NonterminalType nonterminalType in Prolog.Parser.Singleton.Grammar.GetNonterminals())
            {
                foreach (RuleType ruleType in nonterminalType.Rules)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(ruleType.Lhs.Name);
                    sb.Append(" :=");

                    foreach (LanguageElementType terminalType in ruleType.Rhs)
                    {
                        sb.Append(" ");
                        sb.Append(terminalType.Name);
                    }

                    string text = sb.ToString();
                    Console.WriteLine(text);
                    sbGrammar.AppendLine(text);
                }
            }

            System.Windows.Clipboard.SetText(sbGrammar.ToString());
        }
    }
}
