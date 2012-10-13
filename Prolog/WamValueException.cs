/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueException : WamValue
    {
        WamValueException(Exception value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Value = value;
        }

        public static WamValueException Create(Exception value)
        {
            return new WamValueException(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueException(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public Exception Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueException(Value);
        }
    }
}
