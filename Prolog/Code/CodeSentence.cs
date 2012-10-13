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
        public const string ElementName = "CodeSentence";

        public CodeSentence(IEnumerable<CodeComment> comments, CodeCompoundTerm head, IEnumerable<CodeCompoundTerm> body)
        {
            Comments = comments == null ? new CodeCommentList(new List<CodeComment>()) : new CodeCommentList(new List<CodeComment>(comments));

            Head = head;

            Body = body == null ? new CodeCompoundTermList(new List<CodeCompoundTerm>()) : new CodeCompoundTermList(new List<CodeCompoundTerm>(body));
        }

        public static CodeSentence Create(XElement xCodeSentence)
        {
            return new CodeSentence(
                CodeCommentList.Create(xCodeSentence.Element(CodeCommentList.ElementName)), 
                (CodeCompoundTerm)CodeTerm.Create(xCodeSentence.Element(CodeTerm.ElementName)),
                CodeCompoundTermList.Create(xCodeSentence.Element(CodeCompoundTermList.ElementName)));
        }

        public CodeCommentList Comments { get; private set; }
        public CodeCompoundTerm Head { get; private set; }
        public CodeCompoundTermList Body { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeSentence;
            if (rhs == null) return false;

            return Head == rhs.Head && Body == rhs.Body;
        }

        public override int GetHashCode()
        {
            return Head.GetHashCode() ^ Body.GetHashCode();
        }

        public static bool operator ==(CodeSentence lhs, CodeSentence rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;

            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeSentence lhs, CodeSentence rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var comment in Comments)
            {
                sb.Append("/// ");
                sb.Append(comment.Text);
                sb.Append(Environment.NewLine);
            }

            var prefix = ":- ";

            if (Head != null)
            {
                sb.Append(Head);
                prefix = " :-" + Environment.NewLine + "    ";
            }

            foreach (var codeCompoundTerm in Body)
            {
                sb.Append(prefix); prefix = "," + Environment.NewLine + "    ";
                sb.Append(codeCompoundTerm);
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

        public bool Equals(CodeSentence other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Head == other.Head && Body == other.Body;
        }
    }
}
