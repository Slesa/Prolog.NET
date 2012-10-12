/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents a list of the form [Head|Tail].
    /// </summary>
    /// <remarks>The head of a <see cref="CodeList"/> can contain an arbitrary number of elements.</remarks>
    [Serializable]
    public sealed class CodeList : CodeTerm, IEquatable<CodeList>, IImmuttable
    {
        #region Fields

        public new const string ElementName = "CodeList";

        private CodeTermList m_head;
        private CodeTerm m_tail;

        #endregion

        #region Constructors

        public CodeList()
        {
            m_head = new CodeTermList(new List<CodeTerm>());
            m_tail = new CodeCompoundTerm(CodeFunctor.NilFunctor);
        }

        public CodeList(IEnumerable<CodeTerm> head, CodeTerm tail)
        {
            if (head == null)
            {
                throw new ArgumentNullException("head");
            }
            if (tail == null)
            {
                throw new ArgumentNullException("tail");
            }

            if (tail.IsCodeList)
            {
                List<CodeTerm> headTerms = new List<CodeTerm>(head);
                headTerms.AddRange(tail.AsCodeList.Head);

                m_head = new CodeTermList(headTerms);
                m_tail = tail.AsCodeList.Tail;
            }
            else
            {
                m_head = new CodeTermList(new List<CodeTerm>(head));
                m_tail = tail;
            }
        }

        public new static CodeList Create(XElement xCodeList)
        {
            return new CodeList(
                CodeTermList.Create(xCodeList.Element(CodeTermList.ElementName)),
                CodeTerm.Create(xCodeList.Element(CodeTerm.ElementName)));
        }

        #endregion

        #region Public Properties

        public CodeTermList Head
        {
            get { return m_head; }
        }

        public CodeTerm Tail
        {
            get { return m_tail; }
        }

        public override bool IsCodeList
        {
            get { return true; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeList rhs = obj as CodeList;
            if (rhs == null) return false;

            return Head == rhs.Head
                   && Tail == rhs.Tail;
        }

        public override int GetHashCode()
        {
            return Head.GetHashCode()
                   ^ Tail.GetHashCode();
        }

        public static bool operator ==(CodeList lhs, CodeList rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeList lhs, CodeList rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            string prefix = null;
            foreach (CodeTerm codeTerm in Head)
            {
                sb.Append(prefix); prefix = ",";
                sb.Append(codeTerm.ToString());
            }

            if (Tail.IsCodeCompoundTerm
                && Tail.AsCodeCompoundTerm.Functor == CodeFunctor.NilFunctor)
            {
                // no action required.
            }
            else
            {
                sb.Append("|");
                sb.Append(Tail.ToString());
            }

            sb.Append("]");

            return sb.ToString();
        }

        public override XElement ToXElement()
        {
            return ToXElementBase(
                new XElement(ElementName,
                    Head.ToXElement(),
                    Tail.ToXElement()));
        }

        #endregion

        #region IEquatable<CodeList> Members

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeList);
        }

        public bool Equals(CodeList other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Head == other.Head
                   && Tail == other.Tail;
        }

        #endregion
    }
}
