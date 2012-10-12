/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a <see cref="Boolean"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueBoolean : CodeValue, IEquatable<CodeValueBoolean>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeValueBoolean";

        private bool m_value;

        #endregion

        #region Constructors

        public CodeValueBoolean(bool value)
        {
            m_value = value;
        }

        public static new CodeValueBoolean Create(XElement xCodeValueBoolean)
        {
            bool value = bool.Parse(xCodeValueBoolean.Value);

            return new CodeValueBoolean(value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return Value; }
        }

        public bool Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeValueBoolean rhs = obj as CodeValueBoolean;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueBoolean lhs, CodeValueBoolean rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueBoolean lhs, CodeValueBoolean rhs)
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
            return Equals(other as CodeValueBoolean);
        }

        public bool Equals(CodeValueBoolean other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }

        #endregion
    }
}
