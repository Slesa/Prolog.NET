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
        #region Fields

        public new const string ElementName = "CodeValueNumeric";

        #endregion

        #region Constructors

        public new static CodeValueNumeric Create(XElement xCodeValue)
        {
            foreach (XElement xSubtype in xCodeValue.Elements())
            {
                if (xSubtype.Name == CodeValueInteger.ElementName) return CodeValueInteger.Create(xSubtype);
                if (xSubtype.Name == CodeValueDouble.ElementName) return CodeValueDouble.Create(xSubtype);

                throw new InvalidOperationException(string.Format("Unknown subtype element {0}", xSubtype.Name));
            }

            throw new InvalidOperationException("No subtype element specified.");
        }

        #endregion

        #region Public Methods

        public static bool operator ==(CodeValueNumeric lhs, CodeValueNumeric rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeValueNumeric lhs, CodeValueNumeric rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<CodeNumericConstant> Members

        public override bool Equals(CodeValue other)
        {
            return Equals(other as CodeValueNumeric);
        }

        public abstract bool Equals(CodeValueNumeric other);

        #endregion

        #region Hidden Members

        protected override XElement ToXElementBase(XElement content)
        {
            return base.ToXElementBase(
                new XElement(ElementName, content));
        }

        #endregion
    }
}
