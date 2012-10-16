/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.ObjectModel;

namespace Prolog.Workbench
{
    public sealed class Transcript
    {
        public Transcript()
        {
            Entries = new TranscriptEntryList(new ObservableCollection<TranscriptEntry>());
        }

        public TranscriptEntryList Entries { get; private set; }
    }
}
