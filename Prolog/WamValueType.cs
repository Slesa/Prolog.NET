/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamValueType : WamValue
    {
        #region Fields

        private Type m_value;

        #endregion

        #region Constructors

        private WamValueType(Type value)
        {
            m_value = value;
        }

        public static WamValueType Create(Type value)
        {
            return new WamValueType(value);
        }

        public override WamReferenceTarget Clone()
        {
            return new WamValueType(Value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return m_value; }
        }

        public Type Value
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
            return new CodeValueType(Value);
        }

        #endregion
    }
}
