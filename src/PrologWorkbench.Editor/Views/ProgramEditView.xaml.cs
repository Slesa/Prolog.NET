/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Editor.ViewModels;

namespace PrologWorkbench.Editor.Views
{
    public partial class ProgramEditView : UserControl
    {
        public ProgramEditView(ProgramEditViewModel editViewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = editViewModel;
            }
        }
    }
}
