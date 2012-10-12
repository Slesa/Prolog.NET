/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Lingua;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for TraceComponent.xaml
    /// </summary>
    public partial class TraceComponent : UserControl
    {
        #region Fields

        private LinguaTraceListener m_linguaTraceListener;

        #endregion

        #region Constructors

        public TraceComponent()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                AppState.PropertyChanged += AppState_PropertyChanged;
            }
        }

        #endregion

        #region Public Properties

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        #endregion

        #region Command Handlers

        private void CommandClearTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTrace.Text = null;
        }

        private void CommandClearTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTrace.Text))
            {
                e.CanExecute = true;
            }
        }

        #endregion

        #region Event Handlers

        void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TraceEnabled")
            {
                if (AppState.TraceEnabled)
                {
                    if (m_linguaTraceListener == null)
                    {
                        m_linguaTraceListener = new LinguaTraceListener(Dispatcher, WriteTraceLine);
                        LinguaTrace.TraceSource.Listeners.Add(m_linguaTraceListener);
                    }
                }
                else
                {
                    if (m_linguaTraceListener != null)
                    {
                        LinguaTrace.TraceSource.Listeners.Remove(m_linguaTraceListener);
                        m_linguaTraceListener = null;
                    }
                }
            }
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

        private void WriteTraceLine(string text)
        {
            txtTrace.Text += text + System.Environment.NewLine;
        }

        #endregion
    }
}
