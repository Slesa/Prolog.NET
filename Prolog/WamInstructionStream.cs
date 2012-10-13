/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamInstructionStream : IEnumerable<WamInstruction>, IImmuttable
    {
        readonly WamInstruction[] _instructions;
        readonly WamInstructionStreamAttribute[] _attributes;

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

            _instructions = instructions;
            _attributes = attributes;
        }

        public int Length
        {
            get { return _instructions.Length; }
        }

        public WamInstruction this[int index]
        {
            get { return _instructions[index]; }
        }

        public WamInstructionStreamAttribute[] Attributes
        {
            get { return _attributes; }
        }

        public Dictionary<int, string> GetPermanentVariableAssignments()
        {
            var result = new Dictionary<int, string>();
            foreach (var attribute in Attributes)
            {
                var variableAttribute = attribute as WamInstructionStreamVariableAttribute;
                if (variableAttribute != null && variableAttribute.Register.Type == WamInstructionRegisterTypes.Permanent)
                {
                    result.Add(variableAttribute.Register.Id, variableAttribute.Name);
                }
            }
            return result;
        }

        public IEnumerator<WamInstruction> GetEnumerator()
        {
            for (var idx = 0; idx < _instructions.Length; ++idx)
            {
                yield return _instructions[idx];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
