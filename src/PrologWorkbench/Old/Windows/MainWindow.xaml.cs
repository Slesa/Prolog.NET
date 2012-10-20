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
                AppState.View = ControlViews.Transcript;
            }
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
            AppState.View = ControlViews.Debug;
        }

        void CommandViewDebug_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewProgram_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = ControlViews.Program;
        }

        void CommandViewProgram_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewTrace_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = ControlViews.Trace;
        }

        void CommandViewTrace_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void CommandViewTranscript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AppState.View = ControlViews.Transcript;
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
