/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueDouble : WamValue
    {
        #region Fields

        private double m_value;

        #endregion

        #region Constructors

        private WamValueDouble(double value)
        {
            m_value = value;
        }

        public static WamValueDouble Create(double value)
        {
            return new WamValueDouble(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueDouble(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public double Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value.ToString();
        }

        #endregion

        #region Hidden Members

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueDouble(Value);
        }

        #endregion
    }
}
