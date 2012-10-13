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
        public const string ElementName = "CodeCompoundTermList";

        public CodeCompoundTermList(IList<CodeCompoundTerm> items)
            : base(items)
        { }

        private CodeCompoundTermList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public static CodeCompoundTermList Create(XElement xCodeCompoundTermList)
        {
            var codeCompoundTerms = new List<CodeCompoundTerm>();
            foreach (XElement xCodeCompoundTerm in xCodeCompoundTermList.Elements(CodeTerm.ElementName))
            {
                codeCompoundTerms.Add((CodeCompoundTerm)CodeTerm.Create(xCodeCompoundTerm));
            }

            return new CodeCompoundTermList(codeCompoundTerms);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeCompoundTermList;
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
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeCompoundTermList lhs, CodeCompoundTermList rhs)
        {
            return !(lhs == rhs);
        }

        public XElement ToXElement()
        {
            var result = new XElement(ElementName);
            foreach (var codeCompoundTerm in this)
            {
                result.Add(codeCompoundTerm.ToXElement());
            }
            return result;
        }

        public bool Equals(CodeCompoundTermList other)
        {
            if (ReferenceEquals(other, null)) return false;

            if (Count != other.Count) return false;

            for (int idx = 0; idx < Count; ++idx)
            {
                if (this[idx] != other[idx]) return false;
            }

            return true;
        }
    }
}
