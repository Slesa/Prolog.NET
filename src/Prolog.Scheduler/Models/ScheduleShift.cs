namespace Prolog.Scheduler.Models
{
    public class ScheduleShift
    {
        public string Name { get; set; }

        public void Copy(ScheduleShift shift)
        {
            Name = shift.Name;
        }
    }
}