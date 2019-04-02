/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.ObjectModel;

namespace PrologWorkbench.Core.Models
{
    public class Transcript : ObservableCollection<TranscriptEntry>
    {
        public TranscriptEntry AddTranscriptEntry(TranscriptEntryTypes type, string text)
        {
            var entry = new TranscriptEntry(type, text);
            Add(entry);
            return entry;
        }
    }
}
