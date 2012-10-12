/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing an <see cref="Int32"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueInteger : CodeValueNumeric, IEquatable<CodeValueInteger>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeValueInteger";

        private int m_value;

        #endregion

        #region Constructors

        public CodeValueInteger(int value)
        {
            m_value = value;
        }

        public static new CodeValueInteger Create(XElement xCodeValueInteger)
        {
            int value = int.Parse(xCodeValueInteger.Value);

            return new CodeValueInteger(value);
        }

        #endregion

        #region Public Properties

        public override object Object
        {
            get { return Value; }
        }

        public int Value
        {
            get { return m_value; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeValueInteger rhs = obj as CodeValueInteger;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueInteger lhs, CodeValueInteger rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueInteger lhs, CodeValueInteger rhs)
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

        #region IEquatable<CodeIntegerConstant> Members

        public override bool Equals(CodeValueNumeric other)
        {
            return Equals(other as CodeValueInteger);
        }

        public bool Equals(CodeValueInteger other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }

        #endregion
    }
}
