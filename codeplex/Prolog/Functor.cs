/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    /// <summary>
    /// Reprents the functor of a Prolog term or clause.
    /// </summary>
    public sealed class Functor : IEquatable<Functor>, IImmuttable
    {
        #region Fields

        private static string s_cutFunctorName = "!";
        private static string s_listFunctorName = ".";
        private static string s_nilFunctorName = "nil";
        private static string s_pragmaFunctorName = "pragma";
        private static Functor s_cutFunctor = new Functor(CutFunctorName, 0);
        private static Functor s_listFunctor = new Functor(ListFunctorName, 2);
        private static Functor s_nilFunctor = new Functor(NilFunctorName, 0);
        private static Functor s_pragmaFunctor = new Functor(PragmaFunctorName, 2);

        private string m_name;
        private int m_arity;

        #endregion

        #region Constructors

        public Functor(string name, int arity)
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
        }

        public static Functor Create(CodeFunctor codeFunctor)
        {
            return new Functor(codeFunctor.Name, codeFunctor.Arity);
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_name; }
        }

        public int Arity
        {
            get { return m_arity; }
        }

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

        public static string PragmaFunctorName
        {
            get { return s_pragmaFunctorName; }
        }

        public static Functor CutFunctor
        {
            get { return s_cutFunctor; }
        }

        public static Functor ListFunctor
        {
            get { return s_listFunctor; }
        }

        public static Functor NilFunctor
        {
            get { return s_nilFunctor; }
        }

        public static Functor PragmaFunctor
        {
            get { return s_pragmaFunctor; }
        }

        #endregion

        #region Public Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Functor rhs = obj as Functor;
            if (rhs == null) return false;

            return Name == rhs.Name
                   && Arity == rhs.Arity;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode()
                   ^ Arity.GetHashCode();
        }

        public static bool operator ==(Functor lhs, Functor rhs)
        {
            if (object.ReferenceEquals(lhs, rhs)) return true;

            if (object.ReferenceEquals(lhs, null) || object.ReferenceEquals(rhs, null)) return false;

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Functor lhs, Functor rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Name, Arity);
        }

        #endregion

        #region IEquatable<Functor> Members

        public bool Equals(Functor other)
        {
            if (object.ReferenceEquals(other, null)) return false;

            return Name == other.Name
                   && Arity == other.Arity;
        }

        #endregion
    }
}
