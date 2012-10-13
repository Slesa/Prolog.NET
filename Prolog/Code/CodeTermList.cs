/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a list of <see cref="CodeTerm"/> objects.
    /// </summary>
    [Serializable]
    public sealed class CodeTermList : ReadableList<CodeTerm>, IEquatable<CodeTermList>
    {
        public const string ElementName = "CodeTermList";

        public CodeTermList(IList<CodeTerm> items)
            : base(items)
        { }

        private CodeTermList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public static CodeTermList Create(XElement xCodeTermList)
        {
            var codeTerms = new List<CodeTerm>();
            foreach (XElement xCodeTerm in xCodeTermList.Elements(CodeTerm.ElementName))
            {
                codeTerms.Add(CodeTerm.Create(xCodeTerm));
            }

            return new CodeTermList(codeTerms);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeTermList;
            if (rhs == null) return false;

            if (Count != rhs.Count) return false;

            for (var idx = 0; idx < Count; ++idx)
            {
                if (this[idx] != rhs[idx]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var result = 0;

            foreach (var codeTerm in this)
            {
                if (codeTerm != null)
                {
                    result ^= codeTerm.GetHashCode();
                }
            }

            return result;
        }

        public static bool operator ==(CodeTermList lhs, CodeTermList rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeTermList lhs, CodeTermList rhs)
        {
            return !(lhs == rhs);
        }

        public XElement ToXElement()
        {
            var result = new XElement(ElementName);

            foreach (var codeTerm in this)
            {
                result.Add(codeTerm.ToXElement());
            }

            return result;
        }

        public bool Equals(CodeTermList other)
        {
            if (ReferenceEquals(other, null)) return false;

            if (Count != other.Count) return false;

            for (var idx = 0; idx < Count; ++idx)
            {
                if (this[idx] != other[idx]) return false;
            }

            return true;
        }
    }
}
