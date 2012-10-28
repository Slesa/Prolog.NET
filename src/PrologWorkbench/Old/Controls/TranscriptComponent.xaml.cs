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
    /// <summary>F
    /// Interaction logic for TranscriptComponent.xaml
    /// </summary>
    public partial class TranscriptComponent : UserControl
    {
        public TranscriptComponent()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = AppState;
                var notifyCollectionChanged = ctrlTranscriptEntries.Items as INotifyCollectionChanged;
                notifyCollectionChanged.CollectionChanged += ctrlTranscriptEntries_CollectionChanged;
            }
        }

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        void CommandExecuteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var input = txtCommand.Text;
            ProcessInput(input, true);
        }

        void CommandDebugCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var input = txtCommand.Text;
            ProcessInput(input, false);
        }


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

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (this.Parent != null)
            {
                this.Width = double.NaN;
                this.Height = double.NaN;
            }
        }

        void ProcessInput(string input, bool executeQuery)
        {
            // Ignore empty input.
            //
            if (input == null) return;
            input = input.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Request, input);

            var selectedClause = ctrlProgram.SelectedClause;
            var codeSentences = Parser.Parse(input);
            if (codeSentences == null || codeSentences.Length == 0)
            {
                AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.MessageUnrecognizedInput);
                return;
            }

            foreach (var codeSentence in codeSentences)
            {
                if (codeSentence.Head == null) // query
                {
                    var query = new Query(codeSentence);
                    AppState.Machine = PrologMachine.Create(AppState.Program, query);

                    if (executeQuery)
                    {
                        AppState.Machine.RunToSuccess();
                    }
                }
                else // fact or rule
                {
                    if (selectedClause != null && selectedClause.Container.Procedure.Functor == Functor.Create(codeSentence.Head.Functor))
                    {
                        selectedClause.CodeSentence = codeSentence;
                        AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseSuccess);
                    }
                    else
                    {
                        if (AppState.Program.Contains(codeSentence))
                        {
                            AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources. MessageDuplicateClause);
                        }
                        else
                        {
                            AppState.Program.Add(codeSentence);
                            AppState.Transcript.Entries.AddTranscriptEntry(TranscriptEntryTypes.Response, Properties.Resources.ResponseSuccess);
                        }
                    }
                }
            }
        }
    }
}
