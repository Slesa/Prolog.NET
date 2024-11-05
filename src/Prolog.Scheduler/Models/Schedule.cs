/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;
using Prolog.Code;

namespace Prolog.Scheduler.Models
{
    public class Schedule
    {
        static readonly CodeFunctor _workPeriodAssignmentFunctor = new CodeFunctor("workPeriodAssignment", 2);
        static readonly CodeFunctor _workPeriodFunctor = new CodeFunctor("workPeriod", 2);

        public Schedule()
        {
            Monday = new ScheduleDay();
            Tuesday = new ScheduleDay();
            Wednesday = new ScheduleDay();
            Thursday = new ScheduleDay();
            Friday = new ScheduleDay();
        }

        public static Schedule Create(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }

            var result = new Schedule();
            result.ProcessResults(codeTerm);
            return result;
        }

        public ScheduleDay Monday { get; }
        public ScheduleDay Tuesday { get; }
        public ScheduleDay Wednesday { get; }
        public ScheduleDay Thursday { get; }
        public ScheduleDay Friday { get; }
        
        public void Copy(Schedule schedule)
        {
            Monday.Copy(schedule.Monday);
            Tuesday.Copy(schedule.Tuesday);
            Wednesday.Copy(schedule.Wednesday);
            Thursday.Copy(schedule.Thursday);
            Friday.Copy(schedule.Friday);
        }

        /// <summary>
        /// Adds the work period assignments specified by <see cref="CodeTerm"/> to the schedule.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that represents a list of <see cref="CodeCompoundTerm"/> work period assignments.</param>
        void ProcessResults(CodeTerm codeTerm)
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
                throw new ArgumentException("CodeList not specified.", nameof(codeTerm));
            }

            var codeList = codeTerm.AsCodeList;
            foreach (var item in codeList.Head)
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
        void ProcessWorkPeriodAssignment(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", nameof(codeTerm));
            }

            var codeCompoundTerm = codeTerm.AsCodeCompoundTerm;
            if (codeCompoundTerm.Functor != _workPeriodAssignmentFunctor)
            {
                throw new ArgumentException("Functor workPeriodAssignment not specified.", nameof(codeTerm));
            }

            var person = ProcessPerson(codeCompoundTerm.Children[0]);
            var scheduleShift = ProcessWorkPeriod(codeCompoundTerm.Children[1]);
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
        ScheduleShift ProcessWorkPeriod(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", nameof(codeTerm));
            }

            var codeCompoundTerm = codeTerm.AsCodeCompoundTerm;
            if (codeCompoundTerm.Functor != _workPeriodFunctor)
            {
                throw new ArgumentException("Functor workPeriod not specified.", nameof(codeTerm));
            }

            var day = ProcessDay(codeCompoundTerm.Children[0]);
            var shift = ProcessShift(codeCompoundTerm.Children[1]);

            ScheduleDay scheduleDay;
            switch (day)
            {
                case "monday": scheduleDay = Monday; break;
                case "tuesday": scheduleDay = Tuesday; break;
                case "wednesday": scheduleDay = Wednesday; break;
                case "thursday": scheduleDay = Thursday; break;
                case "friday": scheduleDay = Friday; break;
                default:
                    throw new ArgumentException($"Unknown day {day}.", nameof(codeTerm));
            }

            ScheduleShift scheduleShift;
            switch (shift)
            {
                case "first": scheduleShift = scheduleDay.First; break;
                case "second": scheduleShift = scheduleDay.Second; break;
                case "third": scheduleShift = scheduleDay.Third; break;
                default:
                    throw new ArgumentException($"Unknown shift {shift}.", nameof(codeTerm));
            }

            return scheduleShift;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a day.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The day specified by <paramref name="codeTerm"/>.</returns>
        string ProcessDay(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", nameof(codeTerm));
            }

            var codeCompoundTerm = codeTerm.AsCodeCompoundTerm;
            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", nameof(codeTerm));
            }

            return codeCompoundTerm.Functor.Name;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a shift.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The shift specified by <paramref name="codeTerm"/>.</returns>
        string ProcessShift(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", nameof(codeTerm));
            }

            var codeCompoundTerm = codeTerm.AsCodeCompoundTerm;
            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", nameof(codeTerm));
            }

            return codeCompoundTerm.Functor.Name;
        }

        /// <summary>
        /// Processes a <see cref="CodeTerm"/> that specifies a person.
        /// </summary>
        /// <param name="codeTerm">A <see cref="CodeTerm"/> that references an <see cref="CodeAtom"/>.</param>
        /// <returns>The person specified by <paramref name="codeTerm"/>.</returns>
        string ProcessPerson(CodeTerm codeTerm)
        {
            if (codeTerm == null)
            {
                throw new ArgumentNullException(nameof(codeTerm));
            }
            if (!codeTerm.IsCodeCompoundTerm)
            {
                throw new ArgumentException("CodeCompoundTerm not specified.", nameof(codeTerm));
            }

            var codeCompoundTerm = codeTerm.AsCodeCompoundTerm;
            if (codeCompoundTerm.Functor.Arity != 0)
            {
                throw new ArgumentException("Non-zero functor arity specified.", nameof(codeTerm));
            }

            return codeCompoundTerm.Functor.Name;
        }
    }
}