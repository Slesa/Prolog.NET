using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Program.ViewModels;

namespace PrologWorkbench.Program.Views
{
    public partial class ExplorerView : UserControl
    {
        public ExplorerView(ExplorerViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
