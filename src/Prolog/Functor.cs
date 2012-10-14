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
        static readonly string s_cutFunctorName = "!";
        static readonly string s_listFunctorName = ".";
        static readonly string s_nilFunctorName = "nil";
        static readonly string s_pragmaFunctorName = "pragma";
        static readonly Functor s_cutFunctor = new Functor(CutFunctorName, 0);
        static readonly Functor s_listFunctor = new Functor(ListFunctorName, 2);
        static readonly Functor s_nilFunctor = new Functor(NilFunctorName, 0);
        static readonly Functor s_pragmaFunctor = new Functor(PragmaFunctorName, 2);
            
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

            Name = name;
            Arity = arity;
        }

        public static Functor Create(CodeFunctor codeFunctor)
        {
            return new Functor(codeFunctor.Name, codeFunctor.Arity);
        }

        public string Name { get; private set; }
        public int Arity { get; private set; }

        public static string CutFunctorName { get { return s_cutFunctorName; } }
        public static string ListFunctorName { get { return s_listFunctorName; } }

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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var rhs = obj as Functor;
            if (rhs == null) return false;

            return Name == rhs.Name && Arity == rhs.Arity;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Arity.GetHashCode();
        }

        public static bool operator ==(Functor lhs, Functor rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null)) return false;
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

        public bool Equals(Functor other)
        {
            if (ReferenceEquals(other, null)) return false;
            return Name == other.Name && Arity == other.Arity;
        }
    }
}
