/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog.Workbench.Models
{
    public class Transcript
    {
        public List<TranscriptEntry> Entries { get; } = new ();
        
        public TranscriptEntry AddTranscriptEntry(TranscriptEntryTypes type, string text)
        {
            var entry = new TranscriptEntry(type, text);
            Entries.Add(entry);
            return entry;
        }
        
    }
}