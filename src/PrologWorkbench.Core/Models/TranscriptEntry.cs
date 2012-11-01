/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace PrologWorkbench.Core.Models
{
    public class TranscriptEntry
    {
        public TranscriptEntry(TranscriptEntryTypes type, string text)
        {
            Type = type;
            Text = text;
        }

        public TranscriptEntryTypes Type { get; set; }
        public string Text { get; set; }
    }
}
