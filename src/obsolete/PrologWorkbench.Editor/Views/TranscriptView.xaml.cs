using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Editor.ViewModels;

namespace PrologWorkbench.Editor.Views
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
