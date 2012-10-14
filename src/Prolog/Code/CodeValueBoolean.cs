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
        public new const string ElementName = "CodeValueBoolean";

        public CodeValueBoolean(bool value)
        {
            Value = value;
        }

        public static new CodeValueBoolean Create(XElement xCodeValueBoolean)
        {
            var value = bool.Parse(xCodeValueBoolean.Value);
            return new CodeValueBoolean(value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public bool Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueBoolean;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueBoolean lhs, CodeValueBoolean rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

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

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueBoolean);
        }

        public bool Equals(CodeValueBoolean other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
