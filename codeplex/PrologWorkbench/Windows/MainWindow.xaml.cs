/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Prolog.Workbench
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private Views m_view;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                AppState.PropertyChanged += AppState_PropertyChanged;

                DataContext = AppState;

                AppState.View = Views.Transcript;
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

        private void CommandNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                return;
            }

            AppState.Program = new Program();
        }

        private void CommandOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                return;
            }

            Open();
        }

        private void CommandSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                Save(true);
            }
        }

        private void CommandSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null
                && !string.IsNullOrEmpty(AppState.Program.FileName))
            {
                e.CanExecute = true;
            }
        }

        private void CommandSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                SaveAs(true);
            }
        }

        private void CommandSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                e.Handled = true;
            }

            AppState.Program = null;
        }

        private void CommandClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandExit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandExit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandHelpAbout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutDialog dialog = new AboutDialog();
            dialog.ShowDialog();
        }

        private void CommandHelpAbout_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandViewDebug_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Debug;
        }

        private void CommandViewDebug_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandViewProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Program;
        }

        private void CommandViewProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandViewTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Trace;
        }

        private void CommandViewTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandViewTranscript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Transcript;
        }

        private void CommandViewTranscript_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandEnableOptimization_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.Program.IsOptimized = !AppState.Program.IsOptimized;
        }

        private void CommandEnableOptimization_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommandEnableStatistics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.StatisticsEnabled = !AppState.StatisticsEnabled;
        }

        private void CommandEnableStatistics_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandEnableTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.TraceEnabled = !AppState.TraceEnabled;
        }

        private void CommandEnableTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandDebugAddBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.AddBreakpoint();
            }
        }

        private void CommandDebugAddBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanAddBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugClearAllBreakpoints_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ClearAllBreakpoints();
            }
        }

        private void CommandDebugClearAllBreakpoints_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanClearAllBreakpoints)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugClearBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ClearBreakpoint();
            }
        }

        private void CommandDebugClearBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanClearBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugEndProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.EndProgram();
            }
        }

        private void CommandDebugEndProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanEndProgram)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugRestart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.Restart();
            }
        }

        private void CommandDebugRestart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanRestart)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugRunToBacktrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.RunToBacktrack();
            }
        }

        private void CommandDebugRunToBacktrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanRunToBacktrack)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugRunToSuccess_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.RunToSuccess();
            }
        }

        private void CommandDebugRunToSuccess_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanRunToSuccess)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugStepIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepIn();
            }
        }

        private void CommandDebugStepIn_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanStepIn)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugStepOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepOut();
            }
        }

        private void CommandDebugStepOut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanStepOut)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugStepOver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepOver();
            }
        }

        private void CommandDebugStepOver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanStepOver)
            {
                e.CanExecute = true;
            }
        }

        private void CommandDebugToggleBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ToggleBreakpoint();
            }
        }

        private void CommandDebugToggleBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanToggleBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        #endregion

        #region Hidden Members

        private bool EnsureSaved()
        {
            if (AppState.Program == null
                || AppState.Program.IsModified == false)
            {
                return true;
            }

            string fileName = "Untitled";
            if (AppState.Program != null
                && !string.IsNullOrEmpty(AppState.Program.FileName))
            {
                fileName = Path.GetFileName(AppState.Program.FileName);
            }

            SavePromptDialog dialog = new SavePromptDialog();
            dialog.Message = string.Format("Do you want to save {0}?", fileName);
            dialog.Owner = this;
            SavePromptDialogResults result = dialog.ShowDialog();

            if (result == SavePromptDialogResults.Cancel)
            {
                return false;
            }

            if (result == SavePromptDialogResults.Save)
            {
                if (!Save(true))
                {
                    return false;
                }

                return true;
            }

            // Unknown result...
            //
            return true;
        }

        private bool Open()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = Properties.Resources.FileDefaultExt;
            dialog.Filter = Properties.Resources.FileFilter;
            if (dialog.ShowDialog(this) == false)
            {
                return false;
            }

            Program program;
            try
            {
                program = Program.Load(dialog.FileName);
            }
            catch (FileNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }

            AppState.Program = program;

            return true;
        }

        private bool Save(bool forceSave)
        {
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave
                && !AppState.Program.IsModified)
            {
                return true;
            }

            if (string.IsNullOrEmpty(AppState.Program.FileName))
            {
                return SaveAs(forceSave);
            }

            try
            {
                AppState.Program.Save();
            }
            catch (FileNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }

            return true;
        }

        private bool SaveAs(bool forceSave)
        {
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave
                && !AppState.Program.IsModified)
            {
                return true;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = Properties.Resources.FileDefaultExt;
            dialog.Filter = Properties.Resources.FileFilter;
            if (dialog.ShowDialog(this) == false)
            {
                return false;
            }

            try
            {
                AppState.Program.SaveAs(dialog.FileName);
            }
            catch (FileNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (DirectoryNotFoundException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }
            catch (IOException ex)
            {
                CommonExceptionHandlers.HandleException(this, ex);
                return false;
            }

            return true;
        }

        #endregion

        #region Event Handlers

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!EnsureSaved())
            {
                e.Cancel = true;
            }
        }

        void AppState_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "View")
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion
    }
}
