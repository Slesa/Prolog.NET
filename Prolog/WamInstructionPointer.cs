/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal struct WamInstructionPointer : IEquatable<WamInstructionPointer>, IImmuttable
    {
        #region Fields

        private static WamInstructionPointer s_undefined = new WamInstructionPointer();

        private WamInstructionStream m_instructionStream;
        private int m_index;

        #endregion

        #region Constructors

        public WamInstructionPointer(WamInstructionStream instructionStream)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }

            m_instructionStream = instructionStream;
            m_index = 0;
        }

        public WamInstructionPointer(WamInstructionStream instructionStream, int index)
        {
            m_instructionStream = instructionStream;
            m_index = index;
        }

        #endregion

        #region Public Properties

        public static WamInstructionPointer Undefined
        {
            get { return s_undefined; }
        }

        public WamInstructionStream InstructionStream
        {
            get { return m_instructionStream; }
        }

        public int Index
        {
            get { return m_index; }
        }

        public WamInstruction Instruction
        {
            get { return InstructionStream[Index]; }
        }

        #endregion

        #region Public Methods

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
                WamInstructionPointer rhs = (WamInstructionPointer)obj;

                return InstructionStream == rhs.InstructionStream
                       && Index == rhs.Index;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return InstructionStream.GetHashCode()
                   ^ Index.GetHashCode();
        }

        public static bool operator ==(WamInstructionPointer lhs, WamInstructionPointer rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(WamInstructionPointer lhs, WamInstructionPointer rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region IEquatable<Functor> Members

        public bool Equals(WamInstructionPointer other)
        {
            return InstructionStream == other.InstructionStream
                   && Index == other.Index;
        }

        #endregion
    }
}
