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
        public new const string ElementName = "CodeValueException";

        public CodeValueException(Exception value)
        {
            Value = value;
        }

        public static new CodeValueException Create(XElement xCodeValueException)
        {
            throw new NotSupportedException();
        }

        public override object Object
        {
            get { return Value; }
        }

        public Exception Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueException;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueException lhs, CodeValueException rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

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

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueException);
        }

        public bool Equals(CodeValueException other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
