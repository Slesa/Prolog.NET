/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;
using Prolog.Code;

namespace Prolog.Grammar
{
    // Statement ::= Clause
    //           ::= Query
    //
    internal sealed class Statement : PrologNonterminal
    {
        #region Fields

        private CodeSentence m_codeSentence;

        #endregion

        #region Rules

        public static void Rule(Statement lhs, Clause clause)
        {
            lhs.CodeSentence = clause.CodeSentence;
        }

        public static void Rule(Statement lhs, Query query)
        {
            lhs.CodeSentence = query.CodeSentence;
        }

        #endregion

        #region Public Properties

        public CodeSentence CodeSentence
        {
            get { return m_codeSentence; }
            private set { m_codeSentence = value; }
        }

        #endregion
    }
}
