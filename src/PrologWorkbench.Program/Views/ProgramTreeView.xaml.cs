/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Program.ViewModels;

namespace PrologWorkbench.Program.Views
{
    public partial class ProgramTreeView : UserControl
    {

        public ProgramTreeView(ProgramTreeViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }

#if NEVER
        public static readonly RoutedEvent ClauseDoubleClickedEvent = EventManager.RegisterRoutedEvent(
            "ClauseDoubleClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedClauseEventHandler),
            typeof(ProgramTreeView));

        public event RoutedPropertyChangedEventHandler<Clause> ClauseDoubleClicked
        {
            add { AddHandler(ClauseDoubleClickedEvent, value); }
            remove { RemoveHandler(ClauseDoubleClickedEvent, value); }
        }

        protected virtual void OnClauseDoubleClicked(RoutedClauseEventArgs args)
        {
            RaiseEvent(args);
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void CommandCut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var clause = SelectedClause;
            if (clause == null) return;
            Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
            clause.Container.Remove(clause);
        }

        void CommandCut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null)
            {
                e.CanExecute = true;
            }
        }

        void CommandCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var clause = SelectedClause;
            if (clause != null)
            {
                Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
            }
        }

        void CommandCopy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null)
            {
                e.CanExecute = true;
            }
        }

        void CommandPaste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program == null) return;

            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return;

            var codeSentenceObject = dataObject.GetData(CodeSentenceDataObject.CodeSentenceDataFormat);
            if (codeSentenceObject == null) return;

            var codeSentence = codeSentenceObject as CodeSentence;
            if (codeSentence != null || codeSentence.Head != null)
            {
                AppState.Program.Add(codeSentence);
            }
        }

        void CommandPaste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program == null) return;
            
            var dataObject = Clipboard.GetDataObject();
            if (dataObject == null) return;

            if (dataObject.GetDataPresent(CodeSentenceDataObject.CodeSentenceDataFormat))
            {
                e.CanExecute = true;
            }
        }

        void CommandMoveUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var clause = SelectedClause;
            if (clause != null)
            {
                clause.Container.MoveUp(clause);
            }
        }

        void CommandMoveUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null && !SelectedClause.IsFirst)
            {
                e.CanExecute = true;
            }
        }

        void CommandMoveDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var clause = SelectedClause;
            if (clause != null)
            {
                clause.Container.MoveDown(clause);
            }
        }

        void CommandMoveDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null && !SelectedClause.IsLast)
            {
                e.CanExecute = true;
            }
        }

        void ctrlTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedProcedure = ctrlTreeView.SelectedItem as Procedure;
            SelectedClause = ctrlTreeView.SelectedItem as Clause;
        }

        void ctrlTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clause = SelectedClause;
            if (clause != null)
            {
                OnClauseDoubleClicked(new RoutedClauseEventArgs(clause, ClauseDoubleClickedEvent));
            }
        }
#endif
    }
}
