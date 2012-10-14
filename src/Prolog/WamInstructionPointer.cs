/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal struct WamInstructionPointer : IEquatable<WamInstructionPointer>, IImmuttable
    {
        static readonly WamInstructionPointer _undefined = new WamInstructionPointer();

        private readonly WamInstructionStream _instructionStream;
        private readonly int _index;

        public WamInstructionPointer(WamInstructionStream instructionStream)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }
            _instructionStream = instructionStream;
            _index = 0;
        }

        public WamInstructionPointer(WamInstructionStream instructionStream, int index)
        {
            _instructionStream = instructionStream;
            _index = index;
        }

        public static WamInstructionPointer Undefined
        {
            get { return _undefined; }
        }

        public WamInstructionStream InstructionStream
        {
            get { return _instructionStream; }
        }

        public int Index
        {
            get { return _index; }
        }

        public WamInstruction Instruction
        {
            get { return InstructionStream[Index]; }
        }

        public WamInstructionPointer GetNext()
        {
            return new WamInstructionPointer(InstructionStream, Index + 1);
        }

        public WamInstructionPointer GetPrior()
        {
            return new WamInstructionPointer(InstructionStream, Index - 1);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            try
            {
                var rhs = (WamInstructionPointer)obj;
                return InstructionStream == rhs.InstructionStream && Index == rhs.Index;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return InstructionStream.GetHashCode() ^ Index.GetHashCode();
        }

        public static bool operator ==(WamInstructionPointer lhs, WamInstructionPointer rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(WamInstructionPointer lhs, WamInstructionPointer rhs)
        {
            return !(lhs == rhs);
        }

        public bool Equals(WamInstructionPointer other)
        {
            return InstructionStream == other.InstructionStream && Index == other.Index;
        }
    }
}
