/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Xml.Linq;

namespace Prolog.Code
{
    /// <summary>
    /// Represents the functor of a term.
    /// </summary>
    [Serializable]
    public sealed class CodeFunctor : IEquatable<CodeFunctor>, IImmuttable
    {
        public const string ElementName = "CodeFunctor";

        public CodeFunctor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            Arity = 0;
            IsOperator = false;
        }

        public CodeFunctor(string name, int arity)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (arity < 0)
            {
                throw new ArgumentOutOfRangeException("arity");
            }

            Name = name;
            Arity = arity;
            IsOperator = false;
        }

        public CodeFunctor(string name, int arity, bool isOperator)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (arity < 0)
            {
                throw new ArgumentOutOfRangeException("arity");
            }

            Name = name;
            Arity = arity;
            IsOperator = isOperator;
        }

        static CodeFunctor()
        {
            CutFunctorName = "!";
            ListFunctorName = ".";
            NilFunctorName = "nil";
            CutFunctor = new CodeFunctor(CutFunctorName, 0);
            ListFunctor = new CodeFunctor(ListFunctorName, 2);
            NilFunctor = new CodeFunctor(NilFunctorName, 0);
        }

        public static CodeFunctor Create(XElement xCodeFunctor)
        {
            var name = xCodeFunctor.Element("Name").Value;
            var arity = int.Parse(xCodeFunctor.Element("Arity").Value);

            XElement xIsOperator = xCodeFunctor.Element("IsOperator");
            if (xIsOperator != null)
            {
                return new CodeFunctor(name, arity, bool.Parse(xIsOperator.Value));
            }

            return new CodeFunctor(name, arity);
        }

        public static string CutFunctorName { get; private set; }
        public static string ListFunctorName { get; private set; }
        public static string NilFunctorName { get; private set; }
        public static CodeFunctor CutFunctor { get; private set; }
        public static CodeFunctor ListFunctor { get; private set; }
        public static CodeFunctor NilFunctor { get; private set; }
        public string Name { get; private set; }
        public int Arity { get; private set; }
        public bool IsOperator { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            
            var rhs = obj as CodeFunctor;
            if (rhs == null) return false;

            return Name == rhs.Name && Arity == rhs.Arity;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Arity.GetHashCode();
        }

        public static bool operator ==(CodeFunctor lhs, CodeFunctor rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(CodeFunctor lhs, CodeFunctor rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return Name;
        }

        public XElement ToXElement()
        {
            if (IsOperator)
            {
                return new XElement(ElementName,
                    new XElement("Name", Name),
                    new XElement("Arity", Arity.ToString()),
                    new XElement("IsOperator", IsOperator.ToString()));
            }
            else
            {
                return new XElement(ElementName,
                    new XElement("Name", Name),
                    new XElement("Arity", Arity.ToString()));
            }
        }

        public bool Equals(CodeFunctor other)
        {
            if (ReferenceEquals(other, null)) return false;

            return Name == other.Name && Arity == other.Arity;
        }
    }
}
