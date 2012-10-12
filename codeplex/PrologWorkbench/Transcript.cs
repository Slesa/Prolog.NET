/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.ObjectModel;

namespace Prolog.Workbench
{
    public sealed class Transcript
    {
        private TranscriptEntryList m_entries;

        public Transcript()
        {
            m_entries = new TranscriptEntryList(new ObservableCollection<TranscriptEntry>());
        }

        public TranscriptEntryList Entries
        {
            get { return m_entries; }
        }
    }
}
