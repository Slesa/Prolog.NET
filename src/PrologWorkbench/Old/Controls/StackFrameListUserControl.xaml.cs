/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for StackFrameListUserControl.xaml
    /// </summary>
    public partial class StackFrameListUserControl : UserControl, INotifyPropertyChanged
    {
        public StackFrameListUserControl()
        {
            InitializeComponent();

            lstStackFrames.SelectionChanged += lstStackFrames_SelectionChanged;
        }

        public PrologStackFrame SelectedItem
        {
            get { return lstStackFrames.SelectedItem as PrologStackFrame; }
            set { lstStackFrames.SelectedItem = value; }
        }

        public static readonly DependencyProperty StackFramesProperty = DependencyProperty.Register(
            "StackFrames",
            typeof(PrologStackFrameList),
            typeof(StackFrameListUserControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public PrologStackFrameList StackFrames
        {
            get { return (PrologStackFrameList)GetValue(StackFramesProperty); }
            set { SetValue(StackFramesProperty, value); }
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

        void lstStackFrames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
