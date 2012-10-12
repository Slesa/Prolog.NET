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
        #region Fields

        private const string RootElementName = "ProgramTest";
        private const string ProgramNameAttributeName = "programName";
        private const string TestCaseElementName = "TestCase";
        private const string SourceElementName = "Source";

        private string m_programName;

        #endregion

        #region Constructors

        public ProgramTest(string programName)
        {
            m_programName = programName;
        }

        #endregion

        #region Public Properties

        public string ProgramName
        {
            get { return m_programName; }
        }

        public string TestCaseName
        {
            get { return ProgramName + ".xml"; }
        }

        #endregion

        #region Public Methods

        public void CreateTestResults()
        {
            Program program = Program.Load(Path.Combine(Properties.Settings.Default.SamplesFolder, ProgramName));

            XElement xDocument =
                new XElement(RootElementName,
                    new XAttribute(ProgramNameAttributeName, ProgramName));

            foreach (Procedure procedure in program.Procedures)
            {
                foreach (Clause clause in procedure.Clauses)
                {
                    XElement xCodeSentence = clause.CodeSentence.ToXElement();

                    CodeSentence codeSentence = CodeSentence.Create(xCodeSentence);
                    Debug.Assert(codeSentence == clause.CodeSentence);

                    XElement xTest =
                        new XElement(TestCaseElementName,
                            new XElement(SourceElementName, clause.ToString()),
                            xCodeSentence);

                    xDocument.Add(xTest);
                }
            }

            xDocument.Save(Path.Combine(Properties.Settings.Default.TestsFolder, TestCaseName));
        }

        public void ValidateTestResults()
        {
            Console.WriteLine("Validate: {0}", TestCaseName);

            XElement xDocument = XElement.Load(Path.Combine(Properties.Settings.Default.TestsFolder, TestCaseName));

            int testCaseNumber = 0;
            foreach (XElement xTestCase in xDocument.Elements(TestCaseElementName))
            {
                Console.WriteLine("Test case {0}.", ++testCaseNumber);

                string testCaseSource = xTestCase.Element(SourceElementName).Value;
                CodeSentence testCaseCodeSentence = CodeSentence.Create(xTestCase.Element(CodeSentence.ElementName));

                CodeSentence[] codeSentences = Parser.Parse(testCaseSource);
                if (codeSentences == null
                    || codeSentences.Length == 0)
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

        #endregion
    }
}
