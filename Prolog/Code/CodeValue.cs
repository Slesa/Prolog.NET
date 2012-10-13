/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a non-term value.
    /// </summary>
    [Serializable]
    public abstract class CodeValue : CodeTerm, IEquatable<CodeValue>, IImmuttable
    {
        public new const string ElementName = "CodeValue";

        /// <remarks>
        /// See also WamValue.Create.
        /// </remarks>
        public static CodeValue Create(object value)
        {
            if (value == null) return new CodeValueObject(null);

            if (value is bool) return new CodeValueBoolean((bool)value);
            if (value is DateTime) return new CodeValueDateTime((DateTime)value);
            if (value is double) return new CodeValueDouble((double)value);
            if (value is Exception) return new CodeValueException((Exception)value);
            if (value is int) return new CodeValueInteger((int)value);
            if (value is string) return new CodeValueString((string)value);
            if (value is Type) return new CodeValueType((Type)value);

            return new CodeValueObject(value);
        }

        public new static CodeValue Create(XElement xCodeValue)
        {
            foreach (var xSubtype in xCodeValue.Elements())
            {
                if (xSubtype.Name == CodeValueNumeric.ElementName) return CodeValueNumeric.Create(xSubtype);
                if (xSubtype.Name == CodeValueBoolean.ElementName) return CodeValueBoolean.Create(xSubtype);
                if (xSubtype.Name == CodeValueDateTime.ElementName) return CodeValueDateTime.Create(xSubtype);
                if (xSubtype.Name == CodeValueException.ElementName) return CodeValueException.Create(xSubtype);
                if (xSubtype.Name == CodeValueString.ElementName) return CodeValueString.Create(xSubtype);
                if (xSubtype.Name == CodeValueType.ElementName) return CodeValueType.Create(xSubtype);
                if (xSubtype.Name == CodeValueObject.ElementName) return CodeValueObject.Create(xSubtype);

                throw new InvalidOperationException(string.Format("Unknown subtype element {0}", xSubtype.Name));
            }

            throw new InvalidOperationException("No subtype element specified.");
        }

        public override bool IsCodeValue
        {
            get { return true; }
        }

        public abstract object Object { get; }

        public static bool operator ==(CodeValue lhs, CodeValue rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValue lhs, CodeValue rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeValue);
        }

        public abstract bool Equals(CodeValue other);

        protected override XElement ToXElementBase(XElement content)
        {
            return base.ToXElementBase(
                new XElement(ElementName, content));
        }
    }
}
