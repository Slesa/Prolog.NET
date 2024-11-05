/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using Prolog.Workbench.ViewModels;
using ReactiveUI;

namespace Prolog.Workbench.Models
{
    public enum TranscriptEntryTypes
    {
        Request,
        Response
    }

    public class TranscriptEntry : ViewModelBase
    {
        public TranscriptEntry(TranscriptEntryTypes type, string text)
        {
            _type = type;
            _text = text;
        }
        
        TranscriptEntryTypes _type;
        public TranscriptEntryTypes Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        string _text;
        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }
    }
}