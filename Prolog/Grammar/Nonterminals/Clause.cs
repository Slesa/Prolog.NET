/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // Clause ::= OptionalProcedureComments Term OptionalRuleBody
    //
    internal sealed class Clause : PrologNonterminal
    {
        #region Fields

        private CodeSentence m_codeSentence;

        #endregion

        #region Rules

        public static void Rule(Clause lhs, OptionalProcedureComments optionalProcedureComments, Term term, OptionalRuleBody optionalBody)
        {
            IEnumerable<CodeComment> comments = optionalProcedureComments.Comments;
            if (comments == null)
            {
                comments = new CodeComment[] { };
            }

            IEnumerable<CodeCompoundTerm> codeCompoundTerms = optionalBody.CodeCompoundTerms;
            if (codeCompoundTerms == null)
            {
                codeCompoundTerms = new CodeCompoundTerm[] { };
            }

            lhs.CodeSentence = new CodeSentence(comments, term.CodeCompoundTerm, codeCompoundTerms);
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
