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
        #region Constructors

        public DebugComponent()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                AppState.PropertyChanged += AppState_PropertyChanged;
            }
        }

        #endregion

        #region Public Properties

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        #endregion

        #region Event Handlers

        void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Machine")
            {
                if (AppState.Machine != null)
                {
                    AppState.Machine.ExecutionComplete += Machine_ExecutionComplete;
                    AppState.Machine.ExecutionSuspended += Machine_ExecutionSuspended;
                }
            }
        }

        void Machine_ExecutionSuspended(object sender, System.EventArgs e)
        {
            if (AppState.Machine.StackFrames.Count > 0)
            {
                PrologStackFrame stackFrame = AppState.Machine.StackFrames[AppState.Machine.StackFrames.Count - 1];
                ctrlStackFrames.SelectedItem = stackFrame;
            }
        }

        void Machine_ExecutionComplete(object sender, System.EventArgs e)
        {
            if (AppState.Machine.StackFrames.Count > 0)
            {
                PrologStackFrame stackFrame = AppState.Machine.StackFrames[AppState.Machine.StackFrames.Count-1];
                ctrlStackFrames.SelectedItem = stackFrame;
            }
        }

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

        #endregion
    }
}
