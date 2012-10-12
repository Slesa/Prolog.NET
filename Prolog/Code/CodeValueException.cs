/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> object containing a <see cref="Exception"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueException : CodeValue, IEquatable<CodeValueException>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeValueException";

        private Exception m_value;

        #endregion

        #region Constructors

        public CodeValueException(Exception value)
        {
            m_value = value;
        }

        public static new CodeValueException Create(XElement xCodeValueException)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return Value; }
        }

        public Exception Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeValueException rhs = obj as CodeValueException;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueException lhs, CodeValueException rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueException lhs, CodeValueException rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName, Value.ToString()));
        }

        #endregion

        #region IEquatable<CodeStringConstant> Members

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueException);
        }

        public bool Equals(CodeValueException other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }

        #endregion
    }
}
