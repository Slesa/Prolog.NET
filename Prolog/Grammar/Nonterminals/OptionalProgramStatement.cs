/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;
using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalProgramStatement ::= Statement
    //                          ::= nil
    //
    internal sealed class OptionalProgramStatement : PrologNonterminal
    {
        #region Fields

        private CodeSentence m_codeSentence;

        #endregion

        #region Rules

        public static void Rule(OptionalProgramStatement lhs, Statement statement)
        {
            lhs.CodeSentence = statement.CodeSentence;
        }

        public static void Rule(OptionalProgramStatement lhs)
        { }

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
