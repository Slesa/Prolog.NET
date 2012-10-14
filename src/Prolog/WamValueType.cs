/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueType : WamValue
    {
        WamValueType(Type value)
        {
            Value = value;
        }

        public static WamValueType Create(Type value)
        {
            return new WamValueType(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueType(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public Type Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueType(Value);
        }
    }
}
