/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // Query ::= :- StatementElement AdditionalStatementElements
    //
    internal sealed class Query : PrologNonterminal
    {
        #region Fields

        private CodeSentence m_codeSentence;

        #endregion

        #region Rules

        public static void Rule(Query lhs, ColonDash colonDash, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            List<CodeCompoundTerm> codeCompoundTerms = new List<CodeCompoundTerm>();
            codeCompoundTerms.Add(statementElement.CodeCompoundTerm);
            codeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);

            lhs.CodeSentence = new CodeSentence(null, null, codeCompoundTerms);
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
