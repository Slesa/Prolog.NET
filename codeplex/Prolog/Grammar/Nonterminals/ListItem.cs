/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListItem ::= CompoundTermMember
    //
    internal sealed class ListItem : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(ListItem lhs, CompoundTermMember compoundTermMember)
        {
            lhs.CodeTerm = compoundTermMember.CodeTerm;
        }

        #endregion

        #region Public Properties

        public CodeTerm CodeTerm
        {
            get { return m_codeTerm; }
            private set { m_codeTerm = value; }
        }

        #endregion
    }
}
