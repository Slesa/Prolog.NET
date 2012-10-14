/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a <see cref="String"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueString : CodeValue, IEquatable<CodeValueString>, IImmuttable
    {
        public new const string ElementName = "CodeValueString";

        public CodeValueString(string value)
        {
            Value = value ?? string.Empty;
        }

        public static new CodeValueString Create(XElement xCodeValueString)
        {
            var value = xCodeValueString.Value;
            return new CodeValueString(value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public string Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueString;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueString lhs, CodeValueString rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueString lhs, CodeValueString rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            string escapedString = Value.Replace(@"""", @"""""");
            return string.Format(@"""{0}""", escapedString);
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName, Value));
        }

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueString);
        }

        public bool Equals(CodeValueString other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
