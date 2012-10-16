/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog.Scheduler
{
    public class ScheduleDay
    {
        public ScheduleDay()
        {
            Third = new ScheduleShift();
            Second = new ScheduleShift();
            First = new ScheduleShift();
        }

        public ScheduleShift First { get; private set; }
        public ScheduleShift Second { get; private set; }
        public ScheduleShift Third { get; private set; }

        public void Copy(ScheduleDay scheduleDay)
        {
            First.Copy(scheduleDay.First);
            Second.Copy(scheduleDay.Second);
            Third.Copy(scheduleDay.Third);
        }
    }
}
