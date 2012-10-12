/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog.Scheduler
{
    public class Schedule
    {
        #region Fields

        private static CodeFunctor s_workPeriodAssignmentFunctor = new CodeFunctor("workPeriodAssignment", 2);
        private static CodeFunctor s_workPeriodFunctor = new CodeFunctor("workPeriod", 2);

        private ScheduleDay m_monday = new ScheduleDay();
        private ScheduleDay m_tuesday = new ScheduleDay();
        private ScheduleDay m_wednesday = new ScheduleDay();
        private ScheduleDay m_thursday = new ScheduleDay();
        private ScheduleDay m_friday = new ScheduleDay();

        #endregion

        #region Constructors

        public static Schedule Create(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }

            Schedule result = new Schedule();

            result.ProcessResults(codeTerm);

            return result;
        }

        #endregion

        #region PublicProperties

        public ScheduleDay Monday
        {
            get { return m_monday; }
        }

        public ScheduleDay Tuesday
        {
            get { return m_tuesday; }
        }

        public ScheduleDay Wednesday
        {
            get { return m_wednesday; }
        }

        public ScheduleDay Thursday
        {
            get { return m_thursday; }
        }

        public ScheduleDay Friday
        {
            get { return m_friday; }
        }

        #endregion

        #region Public Methods

        public void Copy(Schedule schedule)
        {
            Monday.Copy(schedule.Monday);
            Tuesday.Copy(schedule.Tuesday);
            Wednesday.Copy(schedule.Wednesday);
            Thursday.Copy(schedule.Thursday);
            Friday.Copy(schedule.Friday);
        }

        #endregion

        #region Hidden Members

        /// <summary>
        /// Adds the work period assignments specified by <see cref="CodeTerm"/> to the schedule.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that represents a list of <see cref="CodeCompoundTerm"/> work period assignments.</param>
        private void ProcessResults(CodeTerm codeTerm)
        {
            Debug.Assert(codeTerm != null);

            // If the list is nil, there are no results to process.
            //
            if (codeTerm.IsCodeCompoundTerm
                && codeTerm.AsCodeCompoundTerm.Functor == CodeFunctor.NilFunctor)
            {
                return;
            }

            if (!codeTerm.IsCodeList)
            {
                throw new ArgumentException("CodeList not specified.", "codeTerm");
            }

            CodeList codeList = codeTerm.AsCodeList;

            foreach (CodeTerm item in codeList.Head)
            {
                ProcessWorkPeriodAssignment(item);
            }
        }

        /// <summary>
        /// Adds the work period assignment specified by <see cref="CodeTerm"/> to the schedule.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeCompoundTerm"/>.</param>
        /// <remarks>
        /// Work periods assignment are compound terms of the form <code>workPeriodAssignment(person,workPeriod(day,shift))</code>.
        /// </remarks>
        private void ProcessWorkPeriodAssignment(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", "codeTerm");
            }

            CodeCompoundTerm codeCompoundTerm = codeTerm.AsCodeCompoundTerm;

            if (codeCompoundTerm.Functor != s_workPeriodAssignmentFunctor)
            {
                throw new ArgumentException("Functor workPeriodAssignment not specified.", "codeTerm");
            }

            string person = ProcessPerson(codeCompoundTerm.Children[0]);
            ScheduleShift scheduleShift = ProcessWorkPeriod(codeCompoundTerm.Children[1]);

            scheduleShift.Name = person;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a work period.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeCompoundTerm"/>.</param>
        /// <returns>The <see cref="ScheduleShift"/> specified by <paramref name="codeTerm"/>.</returns>
        /// <remarks>
        /// Work periods are compound terms of the form <code>workPeriod(day,shift)</code>.
        /// </remarks>
        private ScheduleShift ProcessWorkPeriod(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", "codeTerm");
            }

            CodeCompoundTerm codeCompoundTerm = codeTerm.AsCodeCompoundTerm;

            if (codeCompoundTerm.Functor != s_workPeriodFunctor)
            {
                throw new ArgumentException("Functor workPeriod not specified.", "codeTerm");
            }

            string day = ProcessDay(codeCompoundTerm.Children[0]);
            string shift = ProcessShift(codeCompoundTerm.Children[1]);

            ScheduleDay scheduleDay;
            switch (day)
            {
                case "monday": scheduleDay = Monday; break;
                case "tuesday": scheduleDay = Tuesday; break;
                case "wednesday": scheduleDay = Wednesday; break;
                case "thursday": scheduleDay = Thursday; break;
                case "friday": scheduleDay = Friday; break;
                default:
                    throw new ArgumentException(string.Format("Unknown day {0}.", day), "codeTerm");
            }

            ScheduleShift scheduleShift;
            switch (shift)
            {
                case "first": scheduleShift = scheduleDay.First; break;
                case "second": scheduleShift = scheduleDay.Second; break;
                case "third": scheduleShift = scheduleDay.Third; break;
                default:
                    throw new ArgumentException(string.Format("Unknown shift {0}.", shift), "codeTerm");
            }

            return scheduleShift;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a day.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The day specified by <paramref name="codeTerm"/>.</returns>
        private string ProcessDay(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", "codeTerm");
            }

            CodeCompoundTerm codeCompoundTerm = codeTerm.AsCodeCompoundTerm;

            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", "codeTerm");
            }

            return codeCompoundTerm.Functor.Name;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a shift.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The shift specified by <paramref name="codeTerm"/>.</returns>
        private string ProcessShift(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", "codeTerm");
            }

            CodeCompoundTerm codeCompoundTerm = codeTerm.AsCodeCompoundTerm;

            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", "codeTerm");
            }

            return codeCompoundTerm.Functor.Name;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a person.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The person specified by <paramref name="codeTerm"/>.</returns>
        private string ProcessPerson(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException("codeTerm");
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", "codeTerm");
            }

            CodeCompoundTerm codeCompoundTerm = codeTerm.AsCodeCompoundTerm;

            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", "codeTerm");
            }

            return codeCompoundTerm.Functor.Name;
        }

        #endregion
    }
}
