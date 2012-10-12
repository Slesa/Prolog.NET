/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Workbench
{
    public sealed class TranscriptEntry : INotifyPropertyChanged
    {
        private TranscriptEntryTypes m_type;
        private string m_text;

        public TranscriptEntry(TranscriptEntryTypes type, string text)
        {
            m_type = type;
            m_text = text;
        }

        public TranscriptEntryTypes Type
        {
            get { return m_type; }
            set
            {
                if (value != m_type)
                {
                    m_type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        public string Text
        {
            get { return m_text; }
            set
            {
                if (value != m_text)
                {
                    m_text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
