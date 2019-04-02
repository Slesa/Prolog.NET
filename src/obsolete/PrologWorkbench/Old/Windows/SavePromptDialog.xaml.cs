﻿/* Copyright © 2010 Richard G. Todd.
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
        SavePromptDialogResults _result = SavePromptDialogResults.Cancel;

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
            return _result;
        }

        void OnBtnSaveClick(object sender, RoutedEventArgs e)
        {
            _result = SavePromptDialogResults.Save;
            Close();
        }

        void OnBtnContinueClick(object sender, RoutedEventArgs e)
        {
            _result = SavePromptDialogResults.Continue;
            Close();
        }

        void OnBtnCancelClick(object sender, RoutedEventArgs e)
        {
            _result = SavePromptDialogResults.Cancel;

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
