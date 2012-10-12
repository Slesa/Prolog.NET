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
    /// Represents a term with non-zero arity.
    /// </summary>
    [Serializable]
    public sealed class CodeCompoundTerm : CodeTerm, IEquatable<CodeCompoundTerm>, IImmuttable
    {
        public new const string ElementName = "CodeCompoundTerm";

        static readonly CodeTermList EmptyList = new CodeTermList(new CodeTerm[0]);

        readonly CodeFunctor _functor;
        readonly CodeTermList _children;

        public CodeCompoundTerm(CodeFunctor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            if (functor.Arity != 0)
            {
                throw new ArgumentException("Functor with non-zero arity specified.");
            }

            _functor = functor;
            _children = EmptyList;
        }

        public CodeCompoundTerm(CodeFunctor functor, IEnumerable<CodeTerm> children)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            if (functor.Arity == 0)
            {
                throw new ArgumentException("Functor with zero arity specified.");
            }

            _functor = functor;
            _children = new CodeTermList(new List<CodeTerm>(children));

            if (_children.Count != _functor.Arity)
            {
                throw new ArgumentException("Number of arguments does not match functor arity.");
            }
        }

        public new static CodeCompoundTerm Create(XElement xCodeCompoundTerm)
        {
            var codeFunctor = CodeFunctor.Create(xCodeCompoundTerm.Element(CodeFunctor.ElementName));

            if (codeFunctor.Arity == 0)
            {
                return new CodeCompoundTerm(codeFunctor);
            }

            return new CodeCompoundTerm(
                codeFunctor,
                CodeTermList.Create(xCodeCompoundTerm.Element(CodeTermList.ElementName)));
        }

        public CodeFunctor Functor
        {
            get { return _functor; }
        }

        public CodeTermList Children
        {
            get { return _children; }
        }

        public override bool IsCodeCompoundTerm
        {
            get { return true; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as CodeCompoundTerm;
            if (rhs == null) return false;

            return Functor == rhs.Functor && Children == rhs.Children;
        }

        public override int GetHashCode()
        {
            return Functor.GetHashCode() ^ Children.GetHashCode();
        }

        public static bool operator ==(CodeCompoundTerm lhs, CodeCompoundTerm rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeCompoundTerm lhs, CodeCompoundTerm rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Functor == CodeFunctor.ListFunctor)
            {
                var lhs = Children[0].ToString();
                var rhs = Children[1].ToString();

                sb.Append("[");
                sb.Append(lhs);
                if (rhs.StartsWith("[") && rhs.EndsWith("]"))
                {
                    if (rhs == "[]")
                    {
                        // No action required.
                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(rhs.Substring(1, rhs.Length - 2));
                    }
                }
                else
                {
                    sb.Append("|");
                    sb.Append(rhs);
                }
                sb.Append("]");
            }
            else if (Functor.IsOperator && Functor.Arity == 1)
            {
                sb.Append("(");

                sb.Append(Functor.Name);
                sb.Append(" ");
                sb.Append(Children[0]);

                sb.Append(")");
            }
            else if (Functor.IsOperator && Functor.Arity == 2)
            {
                sb.Append("(");

                sb.Append(Children[0]);
                sb.Append(" ");
                sb.Append(Functor.Name);
                sb.Append(" ");
                sb.Append(Children[1]);

                sb.Append(")");
            }
            else
            {
                sb.Append(Functor.Name);
                if (Functor.Arity > 0)
                {
                    sb.Append("(");

                    string prefix = null;
                    foreach (CodeTerm codeTerm in Children)
                    {
                        sb.Append(prefix); prefix = ",";
                        sb.Append(codeTerm.ToString());
                    }

                    sb.Append(")");
                }
            }

            return sb.ToString();
        }

        public override XElement ToXElement()
        {
            return base.ToXElementBase(
                new XElement(ElementName,
                    Functor.ToXElement(),
                    Children.ToXElement()));
        }

        public override bool Equals(CodeTerm other)
        {
            return Equals(other as CodeCompoundTerm);
        }

        public bool Equals(CodeCompoundTerm other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Functor == other.Functor && Children == other.Children;
        }
    }
}
