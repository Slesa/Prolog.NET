/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueDateTime : WamValue
    {
        WamValueDateTime(DateTime value)
        {
            Value = value;
        }

        public static WamValueDateTime Create(DateTime value)
        {
            return new WamValueDateTime(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueDateTime(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public DateTime Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueDateTime(Value);
        }
    }
}
