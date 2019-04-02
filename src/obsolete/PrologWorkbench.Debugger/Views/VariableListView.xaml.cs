using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Debugger.ViewModels;

namespace PrologWorkbench.Debugger.Views
{
    public partial class VariableListView : UserControl
    {
        public VariableListView(VariableListViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
