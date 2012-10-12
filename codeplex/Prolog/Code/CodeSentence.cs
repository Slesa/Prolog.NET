/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Comments do not contribute to object equality.
    /// </remarks>
    [Serializable]
    public sealed class CodeSentence : IEquatable<CodeSentence>, IImmuttable
    {
        #region Fields

        public const string ElementName = "CodeSentence";

        private CodeCommentList m_comments;
        private CodeCompoundTerm m_head;
        private CodeCompoundTermList m_body;

        #endregion

        #region Constructors

        public CodeSentence(IEnumerable<CodeComment> comments, CodeCompoundTerm head, IEnumerable<CodeCompoundTerm> body)
        {
            if (comments == null)
            {
                m_comments = new CodeCommentList(new List<CodeComment>());
            }
            else
            {
                m_comments = new CodeCommentList(new List<CodeComment>(comments));
            }

            m_head = head;

            if (body == null)
            {
                m_body = new CodeCompoundTermList(new List<CodeCompoundTerm>());
            }
            else
            {
                m_body = new CodeCompoundTermList(new List<CodeCompoundTerm>(body));
            }
        }

        public static CodeSentence Create(XElement xCodeSentence)
        {
            return new CodeSentence(
                CodeCommentList.Create(xCodeSentence.Element(CodeCommentList.ElementName)), 
                (CodeCompoundTerm)CodeTerm.Create(xCodeSentence.Element(CodeTerm.ElementName)),
                CodeCompoundTermList.Create(xCodeSentence.Element(CodeCompoundTermList.ElementName)));
        }

        #endregion

        #region Public Properties

        public CodeCommentList Comments
        {
            get { return m_comments; }
        }

        public CodeCompoundTerm Head
        {
            get { return m_head; }
        }

        public CodeCompoundTermList Body
        {
            get { return m_body; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeSentence rhs = obj as CodeSentence;
            if (rhs == null) return false;

            return Head == rhs.Head
                   && Body == rhs.Body;
        }

        public override int GetHashCode()
        {
            return Head.GetHashCode()
                   ^ Body.GetHashCode();
        }

        public static bool operator ==(CodeSentence lhs, CodeSentence rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeSentence lhs, CodeSentence rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (CodeComment comment in Comments)
            {
                sb.Append("/// ");
                sb.Append(comment.Text);
                sb.Append(Environment.NewLine);
            }

            string prefix = ":- ";

            if (Head != null)
            {
                sb.Append(Head.ToString());
                prefix = " :-" + Environment.NewLine + "    ";
            }

            foreach (CodeCompoundTerm codeCompoundTerm in Body)
            {
                sb.Append(prefix); prefix = "," + Environment.NewLine + "    ";
                sb.Append(codeCompoundTerm.ToString());
            }

            return sb.ToString();
        }

        public XElement ToXElement()
        {
            return 
                new XElement(ElementName,
                    Comments.ToXElement(),
                    Head.ToXElement(),
                    Body.ToXElement());
        }

        #endregion

        #region IEquatable<CodeSentence> Members

        public bool Equals(CodeSentence other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Head == other.Head
                   && Body == other.Body;
        }

        #endregion
    }
}
