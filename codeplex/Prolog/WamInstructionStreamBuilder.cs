/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamInstructionStreamBuilder
    {
        #region Fields

        private List<WamInstruction> m_instructions = new List<WamInstruction>();
        private List<WamInstructionStreamAttribute> m_attributes = new List<WamInstructionStreamAttribute>();

        #endregion

        #region Public Properties

        public int LastIndex
        {
            get { return m_instructions.Count - 1; }
        }

        public int NextIndex
        {
            get { return m_instructions.Count; }
        }

        #endregion

        #region Public Methods

        public void Write(WamInstruction instruction)
        {
            m_instructions.Add(instruction);
        }

        public void AddAttribute(WamInstructionStreamAttribute attribute)
        {
            m_attributes.Add(attribute);
        }

        public WamInstructionStream ToInstructionStream()
        {
            return new WamInstructionStream(
                m_instructions.ToArray(),
                m_attributes.ToArray());
        }

        #endregion
    }
}
