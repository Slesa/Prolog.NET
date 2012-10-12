/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog.Scheduler
{
    public class ScheduleDay
    {
        #region Fields

        private ScheduleShift m_first = new ScheduleShift();
        private ScheduleShift m_second = new ScheduleShift();
        private ScheduleShift m_third = new ScheduleShift();

        #endregion

        #region Public Properties

        public ScheduleShift First
        {
            get { return m_first; }
        }

        public ScheduleShift Second
        {
            get { return m_second; }
        }

        public ScheduleShift Third
        {
            get { return m_third; }
        }

        #endregion

        #region Public Methods

        public void Copy(ScheduleDay scheduleDay)
        {
            First.Copy(scheduleDay.First);
            Second.Copy(scheduleDay.Second);
            Third.Copy(scheduleDay.Third);
        }

        #endregion
    }
}
