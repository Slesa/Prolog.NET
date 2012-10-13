/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueInteger : WamValue
    {
        WamValueInteger(int value)
        {
            Value = value;
        }

        public static WamValueInteger Create(int value)
        {
            return new WamValueInteger(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueInteger(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueInteger(Value);
        }
    }
}
