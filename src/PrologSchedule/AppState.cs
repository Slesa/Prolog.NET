/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Scheduler
{
    public class AppState : INotifyPropertyChanged
    {
        internal AppState(App application)
        {
            Application = application;
            Scheduler = new Scheduler();
            _schedule = null;
        }

        public App Application { get; private set; }
        public Scheduler Scheduler { get; private set; }

        Schedule _schedule;
        public Schedule Schedule
        {
            get { return _schedule; }
            set
            {
                if (value != _schedule)
                {
                    _schedule = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Schedule"));
                }
            }
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
