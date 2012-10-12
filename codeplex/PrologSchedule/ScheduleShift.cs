/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Scheduler
{
    public class ScheduleShift : INotifyPropertyChanged
    {
        #region Fields

        private string m_name = string.Empty;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_name; }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (value != m_name)
                {
                    m_name = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("Name"));
                }
            }
        }

        #endregion

        #region Public Methods

        public void Copy(ScheduleShift scheduleShift)
        {
            Name = scheduleShift.Name;
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
