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
    /// Represents a list of <see cref="CodeComment"/> objects.
    /// </summary>
    [Serializable]
    public sealed class CodeCommentList : ReadableList<CodeComment>, IEquatable<CodeCommentList>
    {
        #region Fields

        public const string ElementName = "CodeCommentList";

        #endregion

        #region Constructors

        public CodeCommentList(IList<CodeComment> items)
            : base(items)
        { }

        private CodeCommentList(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public static CodeCommentList Create(XElement xCodeCommentList)
        {
            List<CodeComment> codeComments = new List<CodeComment>();
            foreach (XElement xComment in xCodeCommentList.Elements(CodeComment.ElementName))
            {
                codeComments.Add(CodeComment.Create(xComment));
            }

            return new CodeCommentList(codeComments);
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeCommentList rhs = obj as CodeCommentList;
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

            foreach (CodeComment comment in this)
            {
                result ^= comment.GetHashCode();
            }

            return result;
        }

        public static bool operator ==(CodeCommentList lhs, CodeCommentList rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeCommentList lhs, CodeCommentList rhs)
        {
            return !(lhs == rhs);
        }

        public XElement ToXElement()
        {
            XElement result = new XElement(ElementName);

            foreach (CodeComment codeComment in this)
            {
                result.Add(codeComment.ToXElement());
            }

            return result;
        }

        #endregion

        #region IEquatable<CodeCommentList> Members

        public bool Equals(CodeCommentList other)
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
