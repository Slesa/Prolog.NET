/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamInstructionStreamBuilder
    {
        readonly List<WamInstruction> _instructions = new List<WamInstruction>();
        readonly List<WamInstructionStreamAttribute> _attributes = new List<WamInstructionStreamAttribute>();

        public int LastIndex
        {
            get { return _instructions.Count - 1; }
        }

        public int NextIndex
        {
            get { return _instructions.Count; }
        }

        public void Write(WamInstruction instruction)
        {
            _instructions.Add(instruction);
        }

        public void AddAttribute(WamInstructionStreamAttribute attribute)
        {
            _attributes.Add(attribute);
        }

        public WamInstructionStream ToInstructionStream()
        {
            return new WamInstructionStream(
                _instructions.ToArray(),
                _attributes.ToArray());
        }
    }
}
