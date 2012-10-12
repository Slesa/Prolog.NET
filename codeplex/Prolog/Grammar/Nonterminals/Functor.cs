/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog.Grammar
{
    // Functor ::= atom
    //
    internal sealed class Functor : PrologNonterminal
    {
        #region Fields

        private string m_name;

        #endregion

        #region Rules

        public static void Rule(Functor lhs, Atom atom)
        {
            lhs.Name = atom.Text;
        }

        #endregion

        #region Public Properties

        public string Name
        {
            get { return m_name; }
            private set { m_name = value; }
        }

        #endregion
    }
}
