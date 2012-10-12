/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal class WamMachineStepEventArgsState
    {
        #region Fields

        private WamInstructionPointer m_instructionPointer;

        #endregion

        #region Public Properties

        public WamInstructionPointer InstructionPointer
        {
            get { return m_instructionPointer; }
            set { m_instructionPointer = value; }
        }

        #endregion
    }
}
