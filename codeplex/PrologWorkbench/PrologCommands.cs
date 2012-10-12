/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows.Input;

namespace Prolog.Workbench
{
    public static class PrologCommands
    {
        private static RoutedCommand m_debugCommand;
        private static RoutedCommand m_clearTrace;
        private static RoutedCommand m_enableOptimization;
        private static RoutedCommand m_enableStatistics;
        private static RoutedCommand m_enableTrace;
        private static RoutedCommand m_executeCommand;
        private static RoutedCommand m_exit;
        private static RoutedCommand m_helpAbout;
        private static RoutedCommand m_moveDown;
        private static RoutedCommand m_moveUp;
        private static RoutedCommand m_recallCommand;
        private static RoutedCommand m_viewDebug;
        private static RoutedCommand m_viewProgram;
        private static RoutedCommand m_viewTrace;
        private static RoutedCommand m_viewTranscript;

        static PrologCommands()
        {
            m_debugCommand = new RoutedCommand();
            m_debugCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Control | ModifierKeys.Shift));

            m_clearTrace = new RoutedCommand();

            m_enableOptimization = new RoutedCommand();

            m_enableStatistics = new RoutedCommand();

            m_enableTrace = new RoutedCommand();

            m_executeCommand= new RoutedCommand();
            m_executeCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Control));

            m_exit = new RoutedCommand();

            m_helpAbout = new RoutedCommand();

            m_moveDown = new RoutedCommand();
            m_moveDown.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));

            m_moveUp = new RoutedCommand();
            m_moveUp.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));

            m_recallCommand = new RoutedCommand();
            m_recallCommand.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));

            m_viewDebug = new RoutedCommand();
            m_viewDebug.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));

            m_viewProgram = new RoutedCommand();
            m_viewProgram.InputGestures.Add(new KeyGesture(Key.G, ModifierKeys.Control));

            m_viewTrace = new RoutedCommand();
            m_viewTrace.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));

            m_viewTranscript = new RoutedCommand();
            m_viewTranscript.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
        }

        public static RoutedCommand DebugCommand
        {
            get { return m_debugCommand; }
        }

        public static RoutedCommand ClearTrace
        {
            get { return m_clearTrace; }
        }

        public static RoutedCommand EnableOptimization
        {
            get { return m_enableOptimization; }
        }

        public static RoutedCommand EnableStatistics
        {
            get { return m_enableStatistics; }
        }

        public static RoutedCommand EnableTrace
        {
            get { return m_enableTrace; }
        }

        public static RoutedCommand ExecuteCommand
        {
            get { return m_executeCommand; }
        }

        public static RoutedCommand Exit
        {
            get { return m_exit; }
        }

        public static RoutedCommand HelpAbout
        {
            get { return m_helpAbout; }
        }

        public static RoutedCommand MoveDown
        {
            get { return m_moveDown; }
        }

        public static RoutedCommand MoveUp
        {
            get { return m_moveUp; }
        }

        public static RoutedCommand RecallCommand
        {
            get { return m_recallCommand; }
        }

        public static RoutedCommand ViewDebug
        {
            get { return m_viewDebug; }
        }

        public static RoutedCommand ViewProgram
        {
            get { return m_viewProgram; }
        }

        public static RoutedCommand ViewTrace
        {
            get { return m_viewTrace; }
        }

        public static RoutedCommand ViewTranscript
        {
            get { return m_viewTranscript; }
        }
    }
}
