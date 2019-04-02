using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Core.Views
{
    public partial class ModulesView : UserControl
    {
        public ModulesView(ModulesViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
