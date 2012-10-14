/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueBoolean : WamValue
    {
        WamValueBoolean(bool value)
        {
            Value = value;
        }

        public static WamValueBoolean Create(bool value)
        {
            return new WamValueBoolean(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueBoolean(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public bool Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueBoolean(Value);
        }
    }
}
