using System.ComponentModel;
using System.Windows;
using Prolog.Workbench.ViewModels;

namespace Prolog.Workbench.Views
{
    public partial class Shell : Window
    {
        public Shell(ShellViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
