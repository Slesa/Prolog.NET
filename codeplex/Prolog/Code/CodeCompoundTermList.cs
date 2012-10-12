/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Prolog.Code
{
    [Serializable]
    public sealed class CodeCompoundTermList : ReadableList<CodeCompoundTerm>, IEquatable<CodeCompoundTermList>
    {
        #region Fields

        public const string ElementName = "CodeCompoundTermList";

        #endregion

        #region Constructors

        public CodeCompoundTermList(IList<CodeCompoundTerm> items)
            : base(items)
        { }

        private CodeCompoundTermList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public static CodeCompoundTermList Create(XElement xCodeCompoundTermList)
        {
            List<CodeCompoundTerm> codeCompoundTerms = new List<CodeCompoundTerm>();
            foreach (XElement xCodeCompoundTerm in xCodeCompoundTermList.Elements(CodeTerm.ElementName))
            {
                codeCompoundTerms.Add((CodeCompoundTerm)CodeTerm.Create(xCodeCompoundTerm));
            }

            return new CodeCompoundTermList(codeCompoundTerms);
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeCompoundTermList rhs = obj as CodeCompoundTermList;
            if (rhs == null) return false;

            if (Count != rhs.Count) return false;

            for (int idx = 0; idx < Count; ++idx)
            {
                if (this[idx] != rhs[idx]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;

            foreach (CodeCompoundTerm codeCompoundTerm in this)
            {
                if (codeCompoundTerm != null)
                {
                    result ^= codeCompoundTerm.GetHashCode();
                }
            }

            return result;
        }

        public static bool operator ==(CodeCompoundTermList lhs, CodeCompoundTermList rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeCompoundTermList lhs, CodeCompoundTermList rhs)
        {
            return !(lhs == rhs);
        }

        public XElement ToXElement()
        {
            XElement result = new XElement(ElementName);

            foreach (CodeCompoundTerm codeCompoundTerm in this)
            {
                result.Add(codeCompoundTerm.ToXElement());
            }

            return result;
        }

        #endregion

        #region IEquatable<CodeCompoundTermList> Members

        public bool Equals(CodeCompoundTermList other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            if (Count != other.Count) return false;

            for (int idx = 0; idx < Count; ++idx)
            {
                if (this[idx] != other[idx]) return false;
            }

            return true;
        }

        #endregion
    }
}
