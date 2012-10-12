/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueDateTime : WamValue
    {
        #region Fields

        private DateTime m_value;

        #endregion

        #region Constructors

        private WamValueDateTime(DateTime value)
        {
            m_value = value;
        }

        public static WamValueDateTime Create(DateTime value)
        {
            return new WamValueDateTime(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueDateTime(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public DateTime Value
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
            return new CodeValueDateTime(Value);
        }

        #endregion
    }
}
