/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows.Input;

namespace Prolog.Workbench
{
    public static class PrologDebugCommands
    {
        private static RoutedCommand m_addBreakpoint;
        private static RoutedCommand m_clearAllBreakpoints;
        private static RoutedCommand m_clearBreakpoint;
        private static RoutedCommand m_endProgram;
        private static RoutedCommand m_restart;
        private static RoutedCommand m_runToBacktrack;
        private static RoutedCommand m_runToSuccess;
        private static RoutedCommand m_stepIn;
        private static RoutedCommand m_stepOut;
        private static RoutedCommand m_stepOver;
        private static RoutedCommand m_toggleBreakpoint;

        static PrologDebugCommands()
        {
            m_addBreakpoint = new RoutedCommand();

            m_clearAllBreakpoints = new RoutedCommand();

            m_clearBreakpoint = new RoutedCommand();

            m_endProgram = new RoutedCommand();

            m_restart = new RoutedCommand();
            m_restart.InputGestures.Add(new KeyGesture(Key.F5, ModifierKeys.Control | ModifierKeys.Shift));

            m_runToBacktrack = new RoutedCommand();
            m_runToBacktrack.InputGestures.Add(new KeyGesture(Key.F6));

            m_runToSuccess = new RoutedCommand();
            m_runToSuccess.InputGestures.Add(new KeyGesture(Key.F5));

            m_stepIn = new RoutedCommand();
            m_stepIn.InputGestures.Add(new KeyGesture(Key.F11));

            m_stepOut = new RoutedCommand();
            m_stepOut.InputGestures.Add(new KeyGesture(Key.F11, ModifierKeys.Shift));

            m_stepOver = new RoutedCommand();
            m_stepOver.InputGestures.Add(new KeyGesture(Key.F10));

            m_toggleBreakpoint = new RoutedCommand();
        }

        public static RoutedCommand AddBreakpoint
        {
            get { return m_addBreakpoint; }
        }

        public static RoutedCommand ClearAllBreakpoints
        {
            get { return m_clearAllBreakpoints; }
        }

        public static RoutedCommand ClearBreakpoint
        {
            get { return m_clearBreakpoint; }
        }

        public static RoutedCommand EndProgram
        {
            get { return m_endProgram; }
        }

        public static RoutedCommand Restart
        {
            get { return m_restart; }
        }

        public static RoutedCommand RunToBacktrack
        {
            get { return m_runToBacktrack; }
        }

        public static RoutedCommand RunToSuccess
        {
            get { return m_runToSuccess; }
        }

        public static RoutedCommand StepIn
        {
            get { return m_stepIn; }
        }

        public static RoutedCommand StepOut
        {
            get { return m_stepOut; }
        }

        public static RoutedCommand StepOver
        {
            get { return m_stepOver; }
        }

        public static RoutedCommand ToggleBreakpoint
        {
            get { return m_toggleBreakpoint; }
        }
    }
}
