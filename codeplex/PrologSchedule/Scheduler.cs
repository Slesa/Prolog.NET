/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.IO;

using Prolog.Code;

namespace Prolog.Scheduler
{
    public class Scheduler
    {
        #region Fields

        private Program m_program;
        private Query m_query;
        private PrologMachine m_machine;

        #endregion

        #region Public Properties

        public Program Program
        {
            get
            {
                if (m_program == null)
                {
                    string path = Path.Combine(Properties.Settings.Default.SamplesFolder, "schedule.prolog");

                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException(string.Format("{0} not found.  Consider updating SamplesFolder setting during program development.", path));
                    }

                    Program program = Program.Load(path);

                    m_program = program;
                }

                return m_program;
            }
        }

        public Query Query
        {
            get
            {
                if (m_query == null)
                {
                    CodeSentence codeSentenceQuery = Parser.Parse(":- solve(X)")[0];
                    Query query = new Query(codeSentenceQuery);

                    m_query = query;
                }

                return m_query;
            }
        }

        public PrologMachine Machine
        {
            get
            {
                if (m_machine == null)
                {
                    PrologMachine machine = PrologMachine.Create(Program, Query);

                    m_machine = machine;
                }

                return m_machine;
            }
        }

        #endregion

        #region Public Methods

        public void Restart()
        {
            PrologMachine machine = Machine;

            machine.Restart();
        }

        public Schedule Execute()
        {
            PrologMachine machine = Machine;

            if (!machine.CanRunToSuccess)
            {
                return null;
            }

            ExecutionResults executionResults = machine.RunToSuccess();

            if (executionResults != ExecutionResults.Success)
            {
                return null;
            }

            CodeTerm codeTerm = machine.QueryResults.Variables[0].CodeTerm;
            Schedule schedule = Schedule.Create(codeTerm);
            return schedule;
        }

        #endregion
    }
}
