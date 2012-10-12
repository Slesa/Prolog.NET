/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog.Workbench
{
    public sealed class TranscriptEntryList : ReadableList<TranscriptEntry>
    {
        #region Constructors

        public TranscriptEntryList(IList<TranscriptEntry> transcriptEntries)
            : base(transcriptEntries)
        {
        }

        #endregion

        #region Public Methods

        public TranscriptEntry AddTranscriptEntry(TranscriptEntryTypes type, string text)
        {
            TranscriptEntry entry = new TranscriptEntry(type, text);

            Items.Add(entry);

            return entry;
        }

        public void Clear()
        {
            Items.Clear();
        }

        #endregion
    }
}
