/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Core.Views
{
    public partial class ProgramView : UserControl
    {

        public ProgramView(ProgramViewModel viewModel)
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
            typeof(ProgramView));

        public event RoutedPropertyChangedEventHandler<Clause> ClauseDoubleClicked
        {
            add { AddHandler(ClauseDoubleClickedEvent, value); }
            remove { RemoveHandler(ClauseDoubleClickedEvent, value); }
        }

        protected virtual void OnClauseDoubleClicked(RoutedClauseEventArgs args)
        {
            RaiseEvent(args);
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
