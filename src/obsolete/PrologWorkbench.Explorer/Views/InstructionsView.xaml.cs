using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Explorer.ViewModels;

namespace PrologWorkbench.Explorer.Views
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
