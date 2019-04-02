/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Prolog.Code;
using Prolog.Workbench.Models;

namespace Prolog.Workbench
{
    public partial class TranscriptComponent : UserControl
    {


        void txtCommand_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (txtCommand.IsVisible)
            {
                txtCommand.Focus();
            }
        }

        void ctrlProgram_ClauseDoubleClicked(object sender, RoutedClauseEventArgs e)
        {
            if (e.Clause != null)
            {
                txtCommand.Text = e.Clause.CodeSentence.ToString();
            }
        }

        void ctrlTranscriptEntries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ctrlTranscriptEntries.Items.Count > 0)
            {
                ctrlTranscriptEntries.ScrollIntoView(ctrlTranscriptEntries.Items[ctrlTranscriptEntries.Items.Count - 1]);
            }
        }

        void ctrlTranscriptEntries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var transcriptEntry = ctrlTranscriptEntries.SelectedItem as TranscriptEntry;
            if (transcriptEntry != null)
            {
                txtCommand.Text = transcriptEntry.Text;
                txtCommand.Focus();
            }

        }

    }
}
