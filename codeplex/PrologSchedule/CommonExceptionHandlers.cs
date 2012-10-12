/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;

namespace Prolog.Scheduler
{
    public static class CommonExceptionHandlers
    {
        public static void HandleException(Window window, Exception ex)
        {
            try
            {
                string message = string.Format(Properties.Resources.MessageException);

                TaskDialog dialog = new TaskDialog();

                dialog.InstructionText = message;
                //dialog.Text
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.StandardButtons = TaskDialogStandardButtons.Ok;

                dialog.DetailsExpandedText = ex.Message;

                dialog.Caption = App.Current.ApplicationTitle;
                if (window != null)
                {
                    dialog.OwnerWindowHandle = new WindowInteropHelper(window).Handle;
                }

                dialog.Show();
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void HandleException(Window window, FileNotFoundException ex)
        {
            try
            {
                string fileName = Path.GetFileName(ex.FileName);
                string message = string.Format(Properties.Resources.MessageFileNotFoundException, fileName);

                TaskDialog dialog = new TaskDialog();

                dialog.InstructionText = message;
                //dialog.Text
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.StandardButtons = TaskDialogStandardButtons.Ok;

                dialog.DetailsExpandedText = ex.Message;

                dialog.Caption = App.Current.ApplicationTitle;
                if (window != null)
                {
                    dialog.OwnerWindowHandle = new WindowInteropHelper(window).Handle;
                }

                dialog.Show();
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void HandleException(Window window, DirectoryNotFoundException ex)
        {
            try
            {
                string message = string.Format(Properties.Resources.MessageDirectoryNotFoundException);

                TaskDialog dialog = new TaskDialog();

                dialog.InstructionText = message;
                //dialog.Text
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.StandardButtons = TaskDialogStandardButtons.Ok;

                dialog.DetailsExpandedText = ex.Message;

                dialog.Caption = App.Current.ApplicationTitle;
                if (window != null)
                {
                    dialog.OwnerWindowHandle = new WindowInteropHelper(window).Handle;
                }

                dialog.Show();
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void HandleException(Window window, IOException ex)
        {
            try
            {
                string message = string.Format(Properties.Resources.MessageIOException);

                TaskDialog dialog = new TaskDialog();

                dialog.InstructionText = message;
                //dialog.Text
                dialog.Icon = TaskDialogStandardIcon.Error;
                dialog.StandardButtons = TaskDialogStandardButtons.Ok;

                dialog.DetailsExpandedText = ex.Message;

                dialog.Caption = App.Current.ApplicationTitle;
                if (window != null)
                {
                    dialog.OwnerWindowHandle = new WindowInteropHelper(window).Handle;
                }

                dialog.Show();
            }
            catch (PlatformNotSupportedException)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
