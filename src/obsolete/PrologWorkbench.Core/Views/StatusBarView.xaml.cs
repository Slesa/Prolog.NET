using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Core.Views
{
    public partial class StatusBarView : UserControl
    {
        public StatusBarView(StatusBarViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
