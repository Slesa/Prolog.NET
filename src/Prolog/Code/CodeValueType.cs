/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a <see cref="Type"/> value.
    /// </summary>
    public sealed class CodeValueType : CodeValue, IEquatable<CodeValueType>, IImmuttable
    {
        public new const string ElementName = "CodeValueType";

        public CodeValueType(Type value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Value = value;
        }

        public static new CodeValueType Create(XElement xCodeValueType)
        {
            throw new NotSupportedException();
        }

        public override object Object
        {
            get { return Value; }
        }

        public Type Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueType;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueType lhs, CodeValueType rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueType lhs, CodeValueType rhs)
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
            return Equals(other as CodeValueType);
        }

        public bool Equals(CodeValueType other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
