/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Scheduler
{
    public class ScheduleShift : INotifyPropertyChanged
    {
        string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        public void Copy(ScheduleShift scheduleShift)
        {
            Name = scheduleShift.Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
