/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueInteger : WamValue
    {
        #region Fields

        private int m_value;

        #endregion

        #region Constructors

        private WamValueInteger(int value)
        {
            m_value = value;
        }

        public static WamValueInteger Create(int value)
        {
            return new WamValueInteger(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueInteger(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public int Value
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
            return new CodeValueInteger(Value);
        }

        #endregion
    }
}
