/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for DebugComponent.xaml
    /// </summary>
    public partial class DebugComponent : UserControl
    {
        public DebugComponent()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                AppState.PropertyChanged += AppState_PropertyChanged;
            }
        }

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Machine") return;
            if (AppState.Machine == null) return;
            AppState.Machine.ExecutionComplete += Machine_ExecutionComplete;
            AppState.Machine.ExecutionSuspended += Machine_ExecutionSuspended;
        }

        void Machine_ExecutionSuspended(object sender, System.EventArgs e)
        {
            if (AppState.Machine.StackFrames.Count <= 0) return;
            var stackFrame = AppState.Machine.StackFrames[AppState.Machine.StackFrames.Count - 1];
            ctrlStackFrames.SelectedItem = stackFrame;
        }

        void Machine_ExecutionComplete(object sender, System.EventArgs e)
        {
            if (AppState.Machine.StackFrames.Count <= 0) return;
            var stackFrame = AppState.Machine.StackFrames[AppState.Machine.StackFrames.Count-1];
            ctrlStackFrames.SelectedItem = stackFrame;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (Parent == null) return;
            Width = double.NaN;
            Height = double.NaN;
        }
    }
}
