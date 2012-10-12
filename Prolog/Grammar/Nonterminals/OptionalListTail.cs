/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalListTail ::= ListTail
    //                  ::= nil
    //
    internal sealed class OptionalListTail : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(OptionalListTail lhs, ListTail listTail)
        {
            lhs.CodeTerm = listTail.CodeTerm;
        }

        public static void Rule(OptionalListTail lhs)
        { }

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
