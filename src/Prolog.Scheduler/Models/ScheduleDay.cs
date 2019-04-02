namespace Prolog.Scheduler.Models
{
    public class ScheduleDay
    {
        public ScheduleDay()
        {
            Third = new ScheduleShift();
            Second = new ScheduleShift();
            First = new ScheduleShift();
        }

        public ScheduleShift First { get; }
        public ScheduleShift Second { get; }
        public ScheduleShift Third { get; }

        public void Copy(ScheduleDay scheduleDay)
        {
            First.Copy(scheduleDay.First);
            Second.Copy(scheduleDay.Second);
            Third.Copy(scheduleDay.Third);
        }
    }
}