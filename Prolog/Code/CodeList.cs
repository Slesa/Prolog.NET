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
        public new const string ElementName = "CodeList";

        public CodeList()
        {
            Head = new CodeTermList(new List<CodeTerm>());
            Tail = new CodeCompoundTerm(CodeFunctor.NilFunctor);
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
                var headTerms = new List<CodeTerm>(head);
                headTerms.AddRange(tail.AsCodeList.Head);

                Head = new CodeTermList(headTerms);
                Tail = tail.AsCodeList.Tail;
            }
            else
            {
                Head = new CodeTermList(new List<CodeTerm>(head));
                Tail = tail;
            }
        }

        public new static CodeList Create(XElement xCodeList)
        {
            return new CodeList(
                CodeTermList.Create(xCodeList.Element(CodeTermList.ElementName)),
                CodeTerm.Create(xCodeList.Element(CodeTerm.ElementName)));
        }

        public CodeTermList Head { get; private set; }
        public CodeTerm Tail { get; private set; }

        public override bool IsCodeList
        {
            get { return true; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeList;
            if (rhs == null) return false;

            return Head == rhs.Head && Tail == rhs.Tail;
        }

        public override int GetHashCode()
        {
            return Head.GetHashCode() ^ Tail.GetHashCode();
        }

        public static bool operator ==(CodeList lhs, CodeList rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeList lhs, CodeList rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[");

            string prefix = null;
            foreach (var codeTerm in Head)
            {
                sb.Append(prefix); prefix = ",";
                sb.Append(codeTerm);
            }

            if (Tail.IsCodeCompoundTerm
                && Tail.AsCodeCompoundTerm.Functor == CodeFunctor.NilFunctor)
            {
                // no action required.
            }
            else
            {
                sb.Append("|");
                sb.Append(Tail);
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

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeList);
        }

        public bool Equals(CodeList other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Head == other.Head && Tail == other.Tail;
        }
    }
}
