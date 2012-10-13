/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueDouble : WamValue
    {
        WamValueDouble(double value)
        {
            Value = value;
        }

        public static WamValueDouble Create(double value)
        {
            return new WamValueDouble(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueDouble(Value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public double Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueDouble(Value);
        }
    }
}
