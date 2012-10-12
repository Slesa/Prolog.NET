/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Scheduler
{
    public class AppState : INotifyPropertyChanged
    {
        #region Fields

        private App m_application;

        private Scheduler m_scheduler;
        private Schedule m_schedule;

        #endregion

        #region Constructors

        internal AppState(App application)
        {
            m_application = application;

            m_scheduler = new Scheduler();
            m_schedule = null;
        }

        #endregion

        #region Public Properties

        public App Application
        {
            get { return m_application; }
        }

        public Scheduler Scheduler
        {
            get { return m_scheduler; }
        }

        public Schedule Schedule
        {
            get { return m_schedule; }
            set
            {
                if (value != m_schedule)
                {
                    m_schedule = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Schedule"));
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
