/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows.Input;

namespace Prolog.Workbench
{
    public static class PrologCommands
    {
        static PrologCommands()
        {
            DebugCommand = new RoutedCommand();
            DebugCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Control | ModifierKeys.Shift));

            ClearTrace = new RoutedCommand();

            EnableOptimization = new RoutedCommand();

            EnableStatistics = new RoutedCommand();

            EnableTrace = new RoutedCommand();

            ExecuteCommand= new RoutedCommand();
            ExecuteCommand.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.Control));

            Exit = new RoutedCommand();

            HelpAbout = new RoutedCommand();

            MoveDown = new RoutedCommand();
            MoveDown.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));

            MoveUp = new RoutedCommand();
            MoveUp.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));

            RecallCommand = new RoutedCommand();
            RecallCommand.InputGestures.Add(new KeyGesture(Key.M, ModifierKeys.Control));

            ViewDebug = new RoutedCommand();
            ViewDebug.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));

            ViewProgram = new RoutedCommand();
            ViewProgram.InputGestures.Add(new KeyGesture(Key.G, ModifierKeys.Control));

            ViewTrace = new RoutedCommand();
            ViewTrace.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));

            ViewTranscript = new RoutedCommand();
            ViewTranscript.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));
        }

        public static RoutedCommand DebugCommand { get; private set; }
        public static RoutedCommand ClearTrace { get; private set; }
        public static RoutedCommand EnableOptimization { get; private set; }
        public static RoutedCommand EnableStatistics { get; private set; }
        public static RoutedCommand EnableTrace { get; private set; }
        public static RoutedCommand ExecuteCommand { get; private set; }
        public static RoutedCommand Exit { get; private set; }
        public static RoutedCommand HelpAbout { get; private set; }
        public static RoutedCommand MoveDown { get; private set; }
        public static RoutedCommand MoveUp { get; private set; }
        public static RoutedCommand RecallCommand { get; private set; }
        public static RoutedCommand ViewDebug { get; private set; }
        public static RoutedCommand ViewProgram { get; private set; }
        public static RoutedCommand ViewTrace { get; private set; }
        public static RoutedCommand ViewTranscript { get; private set; }
    }
}
