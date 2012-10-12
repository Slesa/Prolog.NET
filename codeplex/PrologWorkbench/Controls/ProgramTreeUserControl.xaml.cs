/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Prolog.Code;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for ProgramTreeUserControl.xaml
    /// </summary>
    public partial class ProgramTreeUserControl : UserControl, INotifyPropertyChanged
    {
        #region Fields

        private Procedure m_selectedProcedure = null;
        private Clause m_selectedClause = null;

        #endregion

        #region Constructors

        public ProgramTreeUserControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = AppState;
            }
        }

        #endregion

        #region Public Properties

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        public Clause SelectedClause
        {
            get { return m_selectedClause; }
            set
            {
                if (value != m_selectedClause)
                {
                    m_selectedClause = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("SelectedClause"));
                }
            }
        }

        public Procedure SelectedProcedure
        {
            get { return m_selectedProcedure; }
            set
            {
                if (value != m_selectedProcedure)
                {
                    m_selectedProcedure = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("SelectedProcedure"));
                }
            }
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent ClauseDoubleClickedEvent = EventManager.RegisterRoutedEvent(
            "ClauseDoubleClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedClauseEventHandler),
            typeof(ProgramTreeUserControl));

        public event RoutedPropertyChangedEventHandler<Clause> ClauseDoubleClicked
        {
            add { AddHandler(ClauseDoubleClickedEvent, value); }
            remove { RemoveHandler(ClauseDoubleClickedEvent, value); }
        }

        protected virtual void OnClauseDoubleClicked(RoutedClauseEventArgs args)
        {
            RaiseEvent(args);
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (this.Parent != null)
            {
                this.Width = double.NaN;
                this.Height = double.NaN;
            }
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Command Handlers

        private void CommandCut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clause clause = SelectedClause;
            if (clause != null)
            {
                Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
                clause.Container.Remove(clause);
            }
        }

        private void CommandCut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandCopy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clause clause = SelectedClause;
            if (clause != null)
            {
                Clipboard.SetDataObject(new CodeSentenceDataObject(clause.CodeSentence), true);
            }
        }

        private void CommandCopy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandPaste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                object codeSentenceObject = dataObject.GetData(CodeSentenceDataObject.CodeSentenceDataFormat);
                if (codeSentenceObject != null)
                {
                    CodeSentence codeSentence = codeSentenceObject as CodeSentence;
                    if (codeSentence != null
                        || codeSentence.Head != null)
                    {
                        AppState.Program.Add(codeSentence);
                    }
                }
            }
        }

        private void CommandPaste_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject.GetDataPresent(CodeSentenceDataObject.CodeSentenceDataFormat))
                {
                    e.CanExecute = true;
                }
            }
        }

        private void CommandMoveUp_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clause clause = SelectedClause;
            if (clause != null)
            {
                clause.Container.MoveUp(clause);
            }
        }

        private void CommandMoveUp_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null
                && !SelectedClause.IsFirst)
            {
                e.CanExecute = true;
            }
        }

        private void CommandMoveDown_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Clause clause = SelectedClause;
            if (clause != null)
            {
                clause.Container.MoveDown(clause);
            }
        }

        private void CommandMoveDown_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedClause != null
                && !SelectedClause.IsLast)
            {
                e.CanExecute = true;
            }
        }

        #endregion

        #region Event Handlers

        private void ctrlTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedProcedure = ctrlTreeView.SelectedItem as Procedure;
            SelectedClause = ctrlTreeView.SelectedItem as Clause;
        }

        private void ctrlTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Clause clause = SelectedClause;
            if (clause != null)
            {
                OnClauseDoubleClicked(new RoutedClauseEventArgs(clause, ClauseDoubleClickedEvent));
            }
        }

        #endregion
    }
}
