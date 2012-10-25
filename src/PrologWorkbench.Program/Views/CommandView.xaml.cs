using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Program.ViewModels;

namespace PrologWorkbench.Program.Views
{
    public partial class CommandView : UserControl
    {
        public CommandView(CommandViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
