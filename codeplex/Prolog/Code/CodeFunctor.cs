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
        #region Fields

        public const string ElementName = "CodeFunctor";

        private static string s_cutFunctorName = "!";
        private static string s_listFunctorName = ".";
        private static string s_nilFunctorName = "nil";
        private static CodeFunctor s_cutFunctor = new CodeFunctor(CutFunctorName, 0);
        private static CodeFunctor s_listFunctor = new CodeFunctor(ListFunctorName, 2);
        private static CodeFunctor s_nilFunctor = new CodeFunctor(NilFunctorName, 0);

        private string m_name;
        private int m_arity;
        private bool m_isOperator;

        #endregion

        #region Constructors

        public CodeFunctor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            m_name = name;
            m_arity = 0;
            m_isOperator = false;
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

            m_name = name;
            m_arity = arity;
            m_isOperator = false;
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

            m_name = name;
            m_arity = arity;
            m_isOperator = isOperator;
        }

        public static CodeFunctor Create(XElement xCodeFunctor)
        {
            string name = xCodeFunctor.Element("Name").Value;
            int arity = int.Parse(xCodeFunctor.Element("Arity").Value);

            XElement xIsOperator = xCodeFunctor.Element("IsOperator");
            if (xIsOperator != null)
            {
                return new CodeFunctor(name, arity, bool.Parse(xIsOperator.Value));
            }

            return new CodeFunctor(name, arity);
        }

        #endregion

        #region Public Properties

        public static string CutFunctorName
        {
            get { return s_cutFunctorName; }
        }

        public static string ListFunctorName
        {
            get { return s_listFunctorName; }
        }

        public static string NilFunctorName
        {
            get { return s_nilFunctorName; }
        }

        public static CodeFunctor CutFunctor
        {
            get { return s_cutFunctor; }
        }

        public static CodeFunctor ListFunctor
        {
            get { return s_listFunctor; }
        }

        public static CodeFunctor NilFunctor
        {
            get { return s_nilFunctor; }
        }

        public string Name
        {
            get { return m_name; }
        }

        public int Arity
        {
            get { return m_arity; }
        }

        public bool IsOperator
        {
            get { return m_isOperator; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            CodeFunctor rhs = obj as CodeFunctor;
            if (rhs == null) return false;

            return Name == rhs.Name
                   && Arity == rhs.Arity;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()
                   ^ Arity.GetHashCode();
        }

        public static bool operator ==(CodeFunctor lhs, CodeFunctor rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

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

        #endregion

        #region IEquatable<CodeFunctor> Members

        public bool Equals(CodeFunctor other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Name == other.Name
                   && Arity == other.Arity;
        }

        #endregion
    }
}
