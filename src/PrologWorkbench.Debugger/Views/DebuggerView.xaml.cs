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
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
