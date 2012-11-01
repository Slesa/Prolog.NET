/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using Prolog;

namespace PrologWorkbench.Core.Models
{
    public sealed class TranscriptEntryList : ReadableList<TranscriptEntry>
    {
        public TranscriptEntryList(IList<TranscriptEntry> transcriptEntries)
            : base(transcriptEntries)
        {
        }

        public TranscriptEntry AddTranscriptEntry(TranscriptEntryTypes type, string text)
        {
            var entry = new TranscriptEntry(type, text);
            Items.Add(entry);
            return entry;
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
