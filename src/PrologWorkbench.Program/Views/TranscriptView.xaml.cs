using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Program.ViewModels;

namespace PrologWorkbench.Program.Views
{
    public partial class TranscriptView : UserControl
    {
        public TranscriptView(TranscriptViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
