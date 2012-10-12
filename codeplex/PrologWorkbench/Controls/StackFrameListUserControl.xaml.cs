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
        #region Constructors

        public StackFrameListUserControl()
        {
            InitializeComponent();

            lstStackFrames.SelectionChanged += lstStackFrames_SelectionChanged;
        }

        #endregion

        #region Public Properties

        public PrologStackFrame SelectedItem
        {
            get { return lstStackFrames.SelectedItem as PrologStackFrame; }
            set { lstStackFrames.SelectedItem = value; }
        }

        #endregion

        #region Dependency Properties

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

        private void lstStackFrames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaisePropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
