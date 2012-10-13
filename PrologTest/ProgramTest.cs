/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using Prolog;
using Prolog.Code;

namespace PrologTest
{
    public class ProgramTest
    {
        const string RootElementName = "ProgramTest";
        const string ProgramNameAttributeName = "programName";
        const string TestCaseElementName = "TestCase";
        const string SourceElementName = "Source";

        public ProgramTest(string programName)
        {
            ProgramName = programName;
        }

        public string ProgramName { get; private set; }

        public string TestCaseName
        {
            get { return ProgramName + ".xml"; }
        }

        public void CreateTestResults()
        {
            var filename = Path.Combine(Properties.Settings.Default.SamplesFolder, ProgramName);
            if (!File.Exists(filename))
            {
                Console.WriteLine("File {0} not found, aborting", filename);
                return;
            }

            var program = Program.Load(filename);
            var xDocument = new XElement(RootElementName, new XAttribute(ProgramNameAttributeName, ProgramName));

            foreach (var procedure in program.Procedures)
            {
                foreach (var clause in procedure.Clauses)
                {
                    var xCodeSentence = clause.CodeSentence.ToXElement();

                    var codeSentence = CodeSentence.Create(xCodeSentence);
                    Debug.Assert(codeSentence == clause.CodeSentence);

                    var xTest = new XElement(TestCaseElementName, new XElement(SourceElementName, clause.ToString()), xCodeSentence);
                    xDocument.Add(xTest);
                }
            }
            xDocument.Save(Path.Combine(Properties.Settings.Default.TestsFolder, TestCaseName));
        }

        public void ValidateTestResults()
        {
            Console.WriteLine("Validate: {0}", TestCaseName);

            var xDocument = XElement.Load(Path.Combine(Properties.Settings.Default.TestsFolder, TestCaseName));

            var testCaseNumber = 0;
            foreach (var xTestCase in xDocument.Elements(TestCaseElementName))
            {
                Console.WriteLine("Test case {0}.", ++testCaseNumber);

                var testCaseSource = xTestCase.Element(SourceElementName).Value;
                var testCaseCodeSentence = CodeSentence.Create(xTestCase.Element(CodeSentence.ElementName));

                var codeSentences = Parser.Parse(testCaseSource);
                if (codeSentences == null || codeSentences.Length == 0)
                {
                    Console.WriteLine("*** Could not parse source.");
                }
                else if (codeSentences.Length > 1)
                {
                    Console.WriteLine("*** More than one CodeSentence returned by parser.");
                }
                else if (testCaseCodeSentence != codeSentences[0])
                {
                    Console.WriteLine("*** CodeSentence mismatch.");
                }
            }
            Console.WriteLine("Validate complete: {0}", TestCaseName);
        }
    }
}
