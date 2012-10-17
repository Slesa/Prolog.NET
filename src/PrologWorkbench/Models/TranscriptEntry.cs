/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Microsoft.Practices.Prism.ViewModel;

namespace Prolog.Workbench.Models
{
    public sealed class TranscriptEntry : NotificationObject
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
                if (value == _type) return;
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                RaisePropertyChanged(() => Text);
            }
        }
    }
}
