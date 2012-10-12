/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueException : WamValue
    {
        #region Fields

        private Exception m_value;

        #endregion

        #region Constructors

        private WamValueException(Exception value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            m_value = value;
        }

        public static WamValueException Create(Exception value)
        {
            return new WamValueException(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueException(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public Exception Value
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
            return new CodeValueException(Value);
        }

        #endregion
    }
}
