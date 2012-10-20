/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for ProgramExplorerUserControl.xaml
    /// </summary>
    public partial class ProgramExplorerComponent : UserControl
    {
        public ProgramExplorerComponent()
        {
            InitializeComponent();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            if (Parent == null) return;
            Width = double.NaN;
            Height = double.NaN;
        }
    }
}
