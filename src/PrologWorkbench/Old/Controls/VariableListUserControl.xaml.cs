/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for VariableListUserControl.xaml
    /// </summary>
    public partial class VariableListUserControl : UserControl
    {
        public VariableListUserControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VariablesProperty = DependencyProperty.Register(
            "Variables",
            typeof(PrologVariableList),
            typeof(VariableListUserControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public PrologVariableList Variables
        {
            get { return (PrologVariableList)GetValue(VariablesProperty); }
            set { SetValue(VariablesProperty, value); }
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            "Caption",
            typeof(string),
            typeof(VariableListUserControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (this.Parent != null)
            {
                this.Width = double.NaN;
                this.Height = double.NaN;
            }
        }
    }
}
