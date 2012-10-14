/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueObject : WamValue
    {
        private readonly Object _value;

        private WamValueObject(Object value)
        {
            _value = value;
        }

        public static WamValueObject Create(Object value)
        {
            return new WamValueObject(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueObject(Object);
        }

        public override object Object
        {
            get { return _value; }
        }

        public override string ToString()
        {
            return string.Format("{0}", Object);
        }

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueObject(Object);
        }
    }
}
