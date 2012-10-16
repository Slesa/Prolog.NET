/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;

namespace Prolog.Workbench
{
    public sealed class TranscriptEntry : INotifyPropertyChanged
    {
        public TranscriptEntry(TranscriptEntryTypes type, string text)
        {
            _type = type;
            _text = text;
        }

        TranscriptEntryTypes _type;
        public TranscriptEntryTypes Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != _text)
                {
                    _text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }
    }
}
