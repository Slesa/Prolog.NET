/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a code comment.
    /// </summary>
    [Serializable]
    public sealed class CodeComment
    {
        #region Fields

        public const string ElementName = "CodeComment";

        private string m_text;

        #endregion

        #region Constructors

        public CodeComment(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }

            m_text = text;
        }

        public static CodeComment Create(XElement xCodeComment)
        {
            string text = xCodeComment.Value;

            return new CodeComment(text);
        }

        #endregion

        #region Public Properties

        public string Text
        {
            get { return m_text; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeComment rhs = obj as CodeComment;
            if (rhs == null) return false;

            return Text == rhs.Text;
        }

        public override int GetHashCode()
        {
            int result = 0;

            result ^= Text.GetHashCode();

            return result;
        }

        public static bool operator ==(CodeComment lhs, CodeComment rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeComment lhs, CodeComment rhs)
        {
            return !(lhs == rhs);
        }

        public XElement ToXElement()
        {
            return new XElement(ElementName, Text);
        }

        #endregion

        #region IEquatable<CodeComment> Members

        public bool Equals(CodeComment other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Text == other.Text;
        }

        #endregion
    }
}
