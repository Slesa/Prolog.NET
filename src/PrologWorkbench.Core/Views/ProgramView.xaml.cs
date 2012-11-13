/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Core.ViewModels;

namespace PrologWorkbench.Core.Views
{
    public partial class ProgramView : UserControl
    {

        public ProgramView(ProgramViewModel viewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = viewModel;
            }
        }
    }
}
