/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamInstructionStream : IEnumerable<WamInstruction>, IImmuttable
    {
        #region Fields

        private WamInstruction[] m_instructions;
        private WamInstructionStreamAttribute[] m_attributes;

        #endregion

        #region Constructors

        public WamInstructionStream(WamInstruction[] instructions, WamInstructionStreamAttribute[] attributes)
        {
            if (instructions == null)
            {
                throw new ArgumentNullException("instructions");
            }
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            m_instructions = instructions;
            m_attributes = attributes;
        }

        #endregion

        #region Public Properties

        public int Length
        {
            get { return m_instructions.Length; }
        }

        public WamInstruction this[int index]
        {
            get { return m_instructions[index]; }
        }

        public WamInstructionStreamAttribute[] Attributes
        {
            get { return m_attributes; }
        }

        #endregion

        #region Public Methods

        public Dictionary<int, string> GetPermanentVariableAssignments()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (WamInstructionStreamAttribute attribute in Attributes)
            {
                WamInstructionStreamVariableAttribute variableAttribute = attribute as WamInstructionStreamVariableAttribute;
                if (variableAttribute != null
                    && variableAttribute.Register.Type == WamInstructionRegisterTypes.Permanent)
                {
                    result.Add(variableAttribute.Register.Id, variableAttribute.Name);
                }
            }

            return result;
        }

        #endregion

        #region IEnumerable<Instruction> Members

        public IEnumerator<WamInstruction> GetEnumerator()
        {
            for (int idx = 0; idx < m_instructions.Length; ++idx)
            {
                yield return m_instructions[idx];
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
