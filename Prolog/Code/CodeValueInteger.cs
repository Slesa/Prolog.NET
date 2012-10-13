/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing an <see cref="Int32"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueInteger : CodeValueNumeric, IEquatable<CodeValueInteger>, IImmuttable
    {
        public new const string ElementName = "CodeValueInteger";

        public CodeValueInteger(int value)
        {
            Value = value;
        }

        public static new CodeValueInteger Create(XElement xCodeValueInteger)
        {
            int value = int.Parse(xCodeValueInteger.Value);

            return new CodeValueInteger(value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public int Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueInteger;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueInteger lhs, CodeValueInteger rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueInteger lhs, CodeValueInteger rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName, Value.ToString(CultureInfo.InvariantCulture)));
        }

        public override bool Equals(CodeValueNumeric other)
        {
            return Equals(other as CodeValueInteger);
        }

        public bool Equals(CodeValueInteger other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
