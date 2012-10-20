/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Windows.Input;

namespace Prolog.Workbench
{
    public static class PrologDebugCommands
    {
        static PrologDebugCommands()
        {
            AddBreakpoint = new RoutedCommand();

            ClearAllBreakpoints = new RoutedCommand();
            ClearBreakpoint = new RoutedCommand();
            EndProgram = new RoutedCommand();

            Restart = new RoutedCommand();
            Restart.InputGestures.Add(new KeyGesture(Key.F5, ModifierKeys.Control | ModifierKeys.Shift));

            RunToBacktrack = new RoutedCommand();
            RunToBacktrack.InputGestures.Add(new KeyGesture(Key.F6));

            RunToSuccess = new RoutedCommand();
            RunToSuccess.InputGestures.Add(new KeyGesture(Key.F5));

            StepIn = new RoutedCommand();
            StepIn.InputGestures.Add(new KeyGesture(Key.F11));

            StepOut = new RoutedCommand();
            StepOut.InputGestures.Add(new KeyGesture(Key.F11, ModifierKeys.Shift));

            StepOver = new RoutedCommand();
            StepOver.InputGestures.Add(new KeyGesture(Key.F10));

            ToggleBreakpoint = new RoutedCommand();
        }

        public static RoutedCommand AddBreakpoint { get; private set; }
        public static RoutedCommand ClearAllBreakpoints { get; private set; }
        public static RoutedCommand ClearBreakpoint { get; private set; }
        public static RoutedCommand EndProgram { get; private set; }
        public static RoutedCommand Restart { get; private set; }
        public static RoutedCommand RunToBacktrack { get; private set; }
        public static RoutedCommand RunToSuccess { get; private set; }
        public static RoutedCommand StepIn { get; private set; }
        public static RoutedCommand StepOut { get; private set; }
        public static RoutedCommand StepOver { get; private set; }
        public static RoutedCommand ToggleBreakpoint { get; private set; }
    }
}
