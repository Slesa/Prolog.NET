using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Editor.ViewModels;

namespace PrologWorkbench.Editor.Views
{
    public partial class TitleBarView : UserControl
    {
        public TitleBarView(TitleBarViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
