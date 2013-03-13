using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Debugger.ViewModels;

namespace PrologWorkbench.Debugger.Views
{
    public partial class InstructionsView : UserControl
    {
        public InstructionsView(InstructionsViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
