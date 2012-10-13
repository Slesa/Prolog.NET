/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueString : WamValue
    {
        private WamValueString(string value)
        {
            Value = value;
        }

        public static WamValueString Create(string value)
        {
            return new WamValueString(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueString(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value;
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueString(Value);
        }
    }
}
