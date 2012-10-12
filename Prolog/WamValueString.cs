/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueString : WamValue
    {
        #region Fields

        private string m_value;

        #endregion

        #region Constructors

        private WamValueString(string value)
        {
            m_value = value;
        }

        public static WamValueString Create(string value)
        {
            return new WamValueString(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueString(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public string Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Value;
        }

        #endregion

        #region Hidden Members

        protected override CodeTerm GetCodeTermBase(WamDeferenceTypes dereferenceType, WamReferenceTargetMapping mapping)
        {
            return new CodeValueString(Value);
        }

        #endregion
    }
}
