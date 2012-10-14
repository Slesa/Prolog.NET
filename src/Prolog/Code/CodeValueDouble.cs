/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a <see cref="Double"/> value.
    /// </summary>
    [Serializable]
    public sealed class CodeValueDouble : CodeValueNumeric, IEquatable<CodeValueDouble>, IImmuttable
    {
        public new const string ElementName = "CodeValueDouble";

        public CodeValueDouble(double value)
        {
            Value = value;
        }

        public static new CodeValueDouble Create(XElement xCodeValueDouble)
        {
            double value = double.Parse(xCodeValueDouble.Value);

            return new CodeValueDouble(value);
        }

        public override object Object
        {
            get { return Value; }
        }

        public double Value { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeValueDouble;
            if (rhs == null) return false;

            return Value == rhs.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(CodeValueDouble lhs, CodeValueDouble rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueDouble lhs, CodeValueDouble rhs)
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
            return Equals(other as CodeValueDouble);
        }

        public bool Equals(CodeValueDouble other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Value == other.Value;
        }
    }
}
