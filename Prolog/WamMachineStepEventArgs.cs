/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal class WamMachineStepEventArgs : EventArgs
    {
        #region Fields

        private WamMachineStepEventArgsState m_state;
        private bool m_breakpoint;

        #endregion

        #region Constructors

        public WamMachineStepEventArgs(WamMachineStepEventArgsState state)
        {
            m_state = state;
        }

        #endregion

        #region Public Properties

        public WamInstructionPointer InstructionPointer
        {
            get { return m_state.InstructionPointer; }
        }

        public bool Breakpoint
        {
            get { return m_breakpoint; }
            set { m_breakpoint = value; }
        }

        #endregion
    }
}
