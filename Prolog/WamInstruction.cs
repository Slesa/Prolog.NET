/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using System.Text;

namespace Prolog
{
    /// <remarks>
    /// Total declared size of member data: 17 bytes
    /// </remarks>
    internal struct WamInstruction : IImmuttable
    {
        readonly WamInstructionOpCodes _opCode; // 1 byte
        WamInstructionRegister _sourceRegister; // 2 bytes
        WamInstructionRegister _targetRegister; // 2 bytes
        readonly Functor _functor; // 4 bytes
        readonly int _index; // 4 bytes
        readonly WamReferenceTarget _referenceTarget; // 4 bytes

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
            _opCode = opCode;
            _sourceRegister = sourceRegister;
            _functor = functor;
            _index = index;
            _targetRegister = targetRegister;
            _referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode)
        {
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = null;
            _index = -1;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = null;
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
            _opCode = opCode;
            _sourceRegister = sourceRegister;
            _functor = functor;
            _index = -1;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = null;
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
            _opCode = opCode;
            _sourceRegister = sourceRegister;
            _functor = null;
            _index = -1;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = referenceTarget;
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
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = functor;
            _index = -1;
            _targetRegister = targetRegister;
            _referenceTarget = null;
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
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = null;
            _index = -1;
            _targetRegister = targetRegister;
            _referenceTarget = referenceTarget;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamInstructionRegister targetRegister)
        {
            if (targetRegister.IsUnused)
            {
                throw new ArgumentException("Invalid value.", "targetRegister");
            }
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = null;
            _index = -1;
            _targetRegister = targetRegister;
            _referenceTarget = null;
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
            _opCode = opCode;
            _sourceRegister = sourceRegister;
            _functor = null;
            _index = -1;
            _targetRegister = targetRegister;
            _referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = functor;
            _index = -1;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = null;
        }

        public WamInstruction(WamInstructionOpCodes opCode, WamReferenceTarget referenceTarget)
        {
            if (referenceTarget == null)
            {
                throw new ArgumentNullException("referenceTarget");
            }
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = null;
            _index = -1;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = referenceTarget;
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
            _opCode = opCode;
            _sourceRegister = WamInstructionRegister.Unused;
            _functor = functor;
            _index = index;
            _targetRegister = WamInstructionRegister.Unused;
            _referenceTarget = null;
        }

        public WamInstructionOpCodes OpCode
        {
            get { return _opCode; }
        }

        public WamInstructionRegister SourceRegister
        {
            get
            {
                if (_sourceRegister.IsUnused)
                {
                    throw new InvalidOperationException("Register is unused.");
                }
                return _sourceRegister;
            }
        }

        public Functor Functor
        {
            get
            {
                if (_functor == null)
                {
                    throw new InvalidOperationException("Functor is unused.");
                }
                return _functor;
            }
        }

        public int Index
        {
            get
            {
                if (_index < 0)
                {
                    throw new InvalidOperationException("Index is unused.");
                }
                return _index;
            }
        }

        public WamInstructionRegister TargetRegister
        {
            get
            {
                if (_targetRegister.IsUnused)
                {
                    throw new InvalidOperationException("Register is unused.");
                }
                return _targetRegister;
            }
        }

        public WamReferenceTarget ReferenceTarget
        {
            get
            {
                return _referenceTarget;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(OpCode.ToString());
            
            var prefix = " ";
            if (_sourceRegister.IsUsed)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(_sourceRegister.ToString());
            }
            if (_functor != null)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(_functor);
            }
            if (_index >= 0)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(_index.ToString(CultureInfo.InvariantCulture));
            }
            if (_referenceTarget != null)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(_referenceTarget);
            }
            if (_targetRegister.IsUsed)
            {
                sb.Append(prefix); prefix = ", ";
                sb.Append(_targetRegister);
            }
            return sb.ToString();
        }
    }
}
