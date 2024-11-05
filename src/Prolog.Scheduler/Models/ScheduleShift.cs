/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

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