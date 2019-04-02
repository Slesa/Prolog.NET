using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    public partial class DebugComponent : UserControl
    {
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

    }
}
