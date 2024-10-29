/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a <see cref="CodeValue"/> containing a numeric value.
    /// </summary>
    [Serializable]
    public abstract class CodeValueNumeric : CodeValue, IEquatable<CodeValueNumeric>, IImmuttable
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CodeValueNumeric)obj);
        }

        public new const string ElementName = "CodeValueNumeric";

        public new static CodeValueNumeric Create(XElement xCodeValue)
        {
            foreach (var xSubtype in xCodeValue.Elements())
            {
                if (xSubtype.Name == CodeValueInteger.ElementName) return CodeValueInteger.Create(xSubtype);
                if (xSubtype.Name == CodeValueDouble.ElementName) return CodeValueDouble.Create(xSubtype);

                throw new InvalidOperationException(string.Format("Unknown subtype element {0}", xSubtype.Name));
            }

            throw new InvalidOperationException("No subtype element specified.");
        }

        public static bool operator ==(CodeValueNumeric lhs, CodeValueNumeric rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueNumeric lhs, CodeValueNumeric rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueNumeric);
        }

        public abstract bool Equals(CodeValueNumeric other);

        protected override XElement ToXElementBase(XElement content)
        {
            return base.ToXElementBase(
                new XElement(ElementName, content));
        }
    }
}
