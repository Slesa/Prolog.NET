/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;

namespace Prolog.Workbench
{
    public static class CommonExceptionHandlers
    {
        public static void HandleException(Window window, Exception ex)
        {
            try
            {
                var message = string.Format(Properties.Resources.MessageException);
                var dialog = new TaskDialog
                                 {
                                     InstructionText = message,
                                     Icon = TaskDialogStandardIcon.Error,
                                     StandardButtons = TaskDialogStandardButtons.Ok,
                                     DetailsExpandedText = ex.Message,
                                     Caption = App.Current.ApplicationTitle
                                 };
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
                var fileName = Path.GetFileName(ex.FileName);
                var message = string.Format(Properties.Resources.MessageFileNotFoundException, fileName);

                var dialog = new TaskDialog
                                 {
                                     InstructionText = message,
                                     Icon = TaskDialogStandardIcon.Error,
                                     StandardButtons = TaskDialogStandardButtons.Ok,
                                     DetailsExpandedText = ex.Message,
                                     Caption = App.Current.ApplicationTitle
                                 };
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
                var message = string.Format(Properties.Resources.MessageDirectoryNotFoundException);
                var dialog = new TaskDialog
                                 {
                                     InstructionText = message,
                                     Icon = TaskDialogStandardIcon.Error,
                                     StandardButtons = TaskDialogStandardButtons.Ok,
                                     DetailsExpandedText = ex.Message,
                                     Caption = App.Current.ApplicationTitle
                                 };
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
                var message = string.Format(Properties.Resources.MessageIOException);
                var dialog = new TaskDialog
                                 {
                                     InstructionText = message,
                                     Icon = TaskDialogStandardIcon.Error,
                                     StandardButtons = TaskDialogStandardButtons.Ok,
                                     DetailsExpandedText = ex.Message,
                                     Caption = App.Current.ApplicationTitle
                                 };
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
