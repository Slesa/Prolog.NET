using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Debugger.ViewModels;

namespace PrologWorkbench.Debugger.Views
{
    public partial class DebuggerView : UserControl
    {
        public DebuggerView(DebuggerViewModel viewModel)
        {
            InitializeComponent();

            // This is needed to find the right view to activate when calling the debugger in editor
            Name = DebuggerModule.TagDebuggerModule;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
