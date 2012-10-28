using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Editor.ViewModels;

namespace PrologWorkbench.Editor.Views
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
