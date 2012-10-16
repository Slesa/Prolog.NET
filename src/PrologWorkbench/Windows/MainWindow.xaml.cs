/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Microsoft.Win32;
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

        public AppState AppState
        {
            get { return App.Current.AppState; }
        }

        void CommandNew_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                return;
            }
            AppState.Program = new Program();
        }

        void CommandOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                return;
            }
            Open();
        }

        void CommandSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                Save(true);
            }
        }

        void CommandSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null
                && !string.IsNullOrEmpty(AppState.Program.FileName))
            {
                e.CanExecute = true;
            }
        }

        void CommandSaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                SaveAs(true);
            }
        }

        void CommandSaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        void CommandClose_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!EnsureSaved())
            {
                e.Handled = true;
            }

            AppState.Program = null;
        }

        void CommandClose_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        void CommandExit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        void CommandExit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandHelpAbout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new AboutDialog();
            dialog.ShowDialog();
        }

        void CommandHelpAbout_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewDebug_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Debug;
        }

        void CommandViewDebug_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Program;
        }

        void CommandViewProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Trace;
        }

        void CommandViewTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewTranscript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = Views.Transcript;
        }

        void CommandViewTranscript_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandEnableOptimization_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.Program.IsOptimized = !AppState.Program.IsOptimized;
        }

        void CommandEnableOptimization_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Program != null)
            {
                e.CanExecute = true;
            }
        }

        void CommandEnableStatistics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.StatisticsEnabled = !AppState.StatisticsEnabled;
        }

        void CommandEnableStatistics_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandEnableTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.TraceEnabled = !AppState.TraceEnabled;
        }

        void CommandEnableTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandDebugAddBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.AddBreakpoint();
            }
        }

        void CommandDebugAddBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanAddBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugClearAllBreakpoints_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ClearAllBreakpoints();
            }
        }

        void CommandDebugClearAllBreakpoints_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null
                && AppState.Machine.CanClearAllBreakpoints)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugClearBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ClearBreakpoint();
            }
        }

        void CommandDebugClearBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanClearBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugEndProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.EndProgram();
            }
        }

        void CommandDebugEndProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanEndProgram)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugRestart_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.Restart();
            }
        }

        void CommandDebugRestart_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanRestart)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugRunToBacktrack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.RunToBacktrack();
            }
        }

        void CommandDebugRunToBacktrack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanRunToBacktrack)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugRunToSuccess_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.RunToSuccess();
            }
        }

        void CommandDebugRunToSuccess_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanRunToSuccess)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugStepIn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepIn();
            }
        }

        void CommandDebugStepIn_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanStepIn)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugStepOut_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepOut();
            }
        }

        void CommandDebugStepOut_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanStepOut)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugStepOver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.StepOver();
            }
        }

        void CommandDebugStepOver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanStepOver)
            {
                e.CanExecute = true;
            }
        }

        void CommandDebugToggleBreakpoint_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AppState.Machine != null)
            {
                AppState.Machine.ToggleBreakpoint();
            }
        }

        void CommandDebugToggleBreakpoint_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (AppState.Machine != null && AppState.Machine.CanToggleBreakpoint)
            {
                e.CanExecute = true;
            }
        }

        bool EnsureSaved()
        {
            if (AppState.Program == null || AppState.Program.IsModified == false)
            {
                return true;
            }

            var fileName = "Untitled";
            if (AppState.Program != null && !string.IsNullOrEmpty(AppState.Program.FileName))
            {
                fileName = Path.GetFileName(AppState.Program.FileName);
            }

            var dialog = new SavePromptDialog
                             {
                                 Message = string.Format("Do you want to save {0}?", fileName),
                                 Owner = this
                             };
            var result = dialog.ShowDialog();
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
            return true;
        }

        bool Open()
        {
            var dialog = new OpenFileDialog
                             {
                                 DefaultExt = Properties.Resources.FileDefaultExt,
                                 Filter = Properties.Resources.FileFilter
                             };
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

        bool Save(bool forceSave)
        {
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave && !AppState.Program.IsModified)
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

        bool SaveAs(bool forceSave)
        {
            Debug.Assert(AppState.Program != null);
            if (AppState.Program == null)
            {
                return true;
            }

            if (!forceSave && !AppState.Program.IsModified)
            {
                return true;
            }

            var dialog = new SaveFileDialog
                             {
                                 DefaultExt = Properties.Resources.FileDefaultExt,
                                 Filter = Properties.Resources.FileFilter
                             };
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

        void Window_Closing(object sender, CancelEventArgs e)
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
    }
}
