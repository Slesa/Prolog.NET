/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal class WamMachineStepEventArgs : EventArgs
    {
        WamMachineStepEventArgsState _state;

        public WamMachineStepEventArgs(WamMachineStepEventArgsState state)
        {
            _state = state;
        }

        public WamInstructionPointer InstructionPointer
        {
            get { return _state.InstructionPointer; }
        }

        public bool Breakpoint { get; set; }
    }
}
