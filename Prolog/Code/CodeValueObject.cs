/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing an <see cref="Object"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueObject : CodeValue, IEquatable<CodeValueObject>, IImmuttable
    {
        public new const string ElementName = "CodeValueObject";

        public CodeValueObject(object value)
        {
            Value = value;
        }

        public static new CodeValueObject Create(XElement xCodeValueObject)
        {
            throw new NotSupportedException();
        }

        public override object Object
        {
            get { return Value; }
        }

        public object Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueObject;
            if (rhs == null) return false;

            return Object == rhs.Object;
        }

        public override int GetHashCode()
        {
            return Object.GetHashCode();
        }

        public static bool operator ==(CodeValueObject lhs, CodeValueObject rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueObject lhs, CodeValueObject rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return string.Format("{0}", Object);
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName, Value.ToString()));
        }

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueObject);
        }

        public bool Equals(CodeValueObject other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Object == other.Object;
        }
    }
}
