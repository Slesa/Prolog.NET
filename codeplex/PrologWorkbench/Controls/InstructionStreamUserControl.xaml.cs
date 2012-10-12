/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for InstructionStreamUserControl.xaml
    /// </summary>
    public partial class InstructionStreamUserControl : UserControl
    {
        #region Constructors

        public InstructionStreamUserControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty InstructionStreamProperty = DependencyProperty.Register(
            "InstructionStream",
            typeof(PrologInstructionStream),
            typeof(InstructionStreamUserControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public PrologInstructionStream InstructionStream
        {
            get { return (PrologInstructionStream)GetValue(InstructionStreamProperty); }
            set { SetValue(InstructionStreamProperty, value); }
        }

        #endregion

        #region Hidden Members

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);

            if (this.Parent != null)
            {
                this.Width = double.NaN;
                this.Height = double.NaN;
            }
        }

        #endregion
    }
}
