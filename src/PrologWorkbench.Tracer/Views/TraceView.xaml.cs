using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Tracer.ViewModels;

namespace PrologWorkbench.Tracer.Views
{
    public partial class TraceView : UserControl
    {
        public TraceView(TraceViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
