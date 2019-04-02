/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows.Controls;
using PrologWorkbench.Explorer.ViewModels;

namespace PrologWorkbench.Explorer.Views
{
    public partial class ProgramInfoView : UserControl
    {
        public ProgramInfoView(ProgramInfoViewModel infoViewModel)
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DataContext = infoViewModel;
            }
        }
    }
}
