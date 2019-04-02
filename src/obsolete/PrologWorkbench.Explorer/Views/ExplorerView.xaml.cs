using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Explorer.ViewModels;

namespace PrologWorkbench.Explorer.Views
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
