/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text;

namespace Prolog
{
    /// <remarks>
    /// Total declared size of member data: 17 bytes
    /// </remarks>
    internal struct WamInstruction : IImmuttable
    {
        #region Fields

        private WamInstructionOpCodes m_opCode; // 1 byte
        private WamInstructionRegister m_sourceRegister; // 2 bytes
        private WamInstructionRegister m_targetRegister; // 2 bytes
        private Functor m_functor; // 4 bytes
        private int m_index; // 4 bytes
        private WamReferenceTarget m_referenceTarget; // 4 bytes

        #endregion

        #region Constructors

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister sourceRegister, Functor functor, int index, WamInstructionRegister targetRegister)
        {
            if (sourceRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "sourceRegister");
            }
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (index < 0)
            {
                throw new ArgumentException("Invalid index.", "index");
            }
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = sourceRegister;
            m_functor = functor;
            m_index = index;
            m_targetRegister = targetRegister;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode)
        {
            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = null;
            m_index = -1;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister sourceRegister, Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (sourceRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "sourceRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = sourceRegister;
            m_functor = functor;
            m_index = -1;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister sourceRegister, WamReferenceTarget referenceTarget)
        {
            if (referenceTarget == null)
            {
                throw new ArgumentNullException("referenceTarget");
            }
            if (sourceRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "sourceRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = sourceRegister;
            m_functor = null;
            m_index = -1;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = referenceTarget;
        }

        public WamInstruction(WamInstructionOpCodes opCode, Functor functor, WamInstructionRegister targetRegister)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = functor;
            m_index = -1;
            m_targetRegister = targetRegister;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamReferenceTarget referenceTarget, WamInstructionRegister targetRegister)
        {
            if (referenceTarget == null)
            {
                throw new ArgumentNullException("referenceTarget");
            }
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = null;
            m_index = -1;
            m_targetRegister = targetRegister;
            m_referenceTarget = referenceTarget;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister targetRegister)
        {
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = null;
            m_index = -1;
            m_targetRegister = targetRegister;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister sourceRegister, WamInstructionRegister targetRegister)
        {
            if (sourceRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "sourceRegister");
            }
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }

            m_opCode = opCode;
            m_sourceRegister = sourceRegister;
            m_functor = null;
            m_index = -1;
            m_targetRegister = targetRegister;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = functor;
            m_index = -1;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamReferenceTarget referenceTarget)
        {
            if (referenceTarget == null)
            {
                throw new ArgumentNullException("referenceTarget");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = null;
            m_index = -1;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = referenceTarget;
        }

        public WamInstruction(WamInstructionOpCodes opCode, Functor functor, int index)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (index < 0)
            {
                throw new ArgumentException("Invalid index.", "index");
            }

            m_opCode = opCode;
            m_sourceRegister = WamInstructionRegister.Unused;
            m_functor = functor;
            m_index = index;
            m_targetRegister = WamInstructionRegister.Unused;
            m_referenceTarget = null;
        }

        #endregion

        #region Public Properties

        public WamInstructionOpCodes OpCode
        {
            get { return m_opCode; }
        }

        public WamInstructionRegister SourceRegister
        {
            get
            {
                if (m_sourceRegister.IsUnused)
                {
                    throw new InvalidOperationException("Register is unused.");
                }

                return m_sourceRegister;
            }
        }

        public Functor Functor
        {
            get
            {
                if (m_functor == null)
                {
                    throw new InvalidOperationException("Functor is unused.");
                }

                return m_functor;
            }
        }

        public int Index
        {
            get
            {
                if (m_index < 0)
                {
                    throw new InvalidOperationException("Index is unused.");
                }

                return m_index;
            }
        }

        public WamInstructionRegister TargetRegister
        {
            get
            {
                if (m_targetRegister.IsUnused)
                {
                    throw new InvalidOperationException("Register is unused.");
                }

                return m_targetRegister;
            }
        }

        public WamReferenceTarget ReferenceTarget
        {
            get
            {
                return m_referenceTarget;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OpCode.ToString());

            string prefix = " ";

            if (m_sourceRegister.IsUsed)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(m_sourceRegister.ToString());
            }

            if (m_functor != null)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(m_functor.ToString());
            }

            if (m_index >= 0)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(m_index.ToString());
            }

            if (m_referenceTarget != null)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(m_referenceTarget.ToString());
            }

            if (m_targetRegister.IsUsed)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(m_targetRegister.ToString());
            }

            return sb.ToString();
        }

        #endregion
    }
}
