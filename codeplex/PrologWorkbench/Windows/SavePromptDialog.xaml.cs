/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for SavePromptDialog.xaml
    /// </summary>
    public partial class SavePromptDialog : Window
    {
        private SavePromptDialogResults m_result = SavePromptDialogResults.Cancel;

        public SavePromptDialog()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return txtMessage.Text; }
            set { txtMessage.Text = value; }
        }

        public new SavePromptDialogResults ShowDialog()
        {
            base.ShowDialog();

            return m_result;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            m_result = SavePromptDialogResults.Save;

            Close();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            m_result = SavePromptDialogResults.Continue;

            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            m_result = SavePromptDialogResults.Cancel;

            Close();
        }
    }

    public enum SavePromptDialogResults
    {
        Save,
        Continue,
        Cancel
    }
}
