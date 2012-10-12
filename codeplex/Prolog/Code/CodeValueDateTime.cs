/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a <see cref="DateTime"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueDateTime : CodeValue, IEquatable<CodeValueDateTime>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeValueDateTime";

        private DateTime m_value;

        #endregion

        #region Constructors

        public CodeValueDateTime(DateTime value)
        {
            m_value = value;
        }

        public static new CodeValueDateTime Create(XElement xCodeValueDateTime)
        {
            DateTime value = DateTime.Parse(xCodeValueDateTime.Value);

            return new CodeValueDateTime(value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return Value; }
        }

        public DateTime Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeValueDateTime rhs = obj as CodeValueDateTime;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueDateTime lhs, CodeValueDateTime rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueDateTime lhs, CodeValueDateTime rhs)
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
            return Equals(other as CodeValueDateTime);
        }

        public bool Equals(CodeValueDateTime other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }

        #endregion
    }
}
