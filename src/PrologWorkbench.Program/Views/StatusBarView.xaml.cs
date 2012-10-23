using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Program.ViewModels;

namespace PrologWorkbench.Program.Views
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
