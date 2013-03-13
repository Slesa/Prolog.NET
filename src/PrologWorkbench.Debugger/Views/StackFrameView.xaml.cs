using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Debugger.ViewModels;

namespace PrologWorkbench.Debugger.Views
{
    public partial class StackFrameView : UserControl
    {
        public StackFrameView(StackFrameViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
