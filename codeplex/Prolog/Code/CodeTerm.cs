/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Prolog.Code
{
    [Serializable]
    public abstract class CodeTerm : IEquatable<CodeTerm>, IImmuttable
    {
        #region Fields

        public const string ElementName = "CodeTerm";

        #endregion

        #region Constructors

        public static CodeTerm Create(XElement xCodeTerm)
        {
            foreach (XElement xSubtype in xCodeTerm.Elements())
            {
                if (xSubtype.Name == CodeValue.ElementName) return CodeValue.Create(xSubtype);
                if (xSubtype.Name == CodeVariable.ElementName) return CodeVariable.Create(xSubtype);
                if (xSubtype.Name == CodeCompoundTerm.ElementName) return CodeCompoundTerm.Create(xSubtype);
                if (xSubtype.Name == CodeList.ElementName) return CodeList.Create(xSubtype);

                throw new InvalidOperationException(string.Format("Unknown subtype element {0}", xSubtype.Name));
            }

            throw new InvalidOperationException("No subtype element specified.");
        }

        #endregion

        #region Public Properties

        public virtual bool IsCodeCompoundTerm
        {
            get { return false; }
        }

        public virtual bool IsCodeVariable
        {
            get { return false; }
        }

        public virtual bool IsCodeValue
        {
            get { return false; }
        }

        public virtual bool IsCodeList
        {
            get { return false; }
        }

        public CodeCompoundTerm AsCodeCompoundTerm
        {
            get { return (CodeCompoundTerm)this; }
        }

        public CodeVariable AsCodeVariable
        {
            get { return (CodeVariable)this; }
        }

        public CodeValue AsCodeValue
        {
            get { return (CodeValue)this; }
        }

        public CodeList AsCodeList
        {
            get { return (CodeList)this; }
        }

        #endregion

        #region Public Methods

        public static bool operator ==(CodeTerm lhs, CodeTerm rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeTerm lhs, CodeTerm rhs)
        {
            return !(lhs == rhs);
        }

        public abstract XElement ToXElement();

        #endregion

        #region IEquatable<CodeTerm> Members

        public abstract bool Equals(CodeTerm other);

        #endregion

        #region Hidden Members

        protected virtual XElement ToXElementBase(XElement content)
        {
            return new XElement(ElementName, content);
        }

        #endregion
    }
}
