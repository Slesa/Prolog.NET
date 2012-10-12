/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueBoolean : WamValue
    {
        #region Fields

        private bool m_value;

        #endregion

        #region Constructors

        private WamValueBoolean(bool value)
        {
            m_value = value;
        }

        public static WamValueBoolean Create(bool value)
        {
            return new WamValueBoolean(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueBoolean(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public bool Value
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
            return new CodeValueBoolean(Value);
        }

        #endregion
    }
}
