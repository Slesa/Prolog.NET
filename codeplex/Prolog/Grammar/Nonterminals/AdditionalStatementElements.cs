/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // AdditionalStatementElements ::= , StatementElement AdditionalStatementElements
    //                             ::= nil
    //
    internal sealed class AdditionalStatementElements : PrologNonterminal
    {
        #region Fields

        private List<CodeCompoundTerm> m_codeCompoundTerms = new List<CodeCompoundTerm>();

        #endregion

        #region Rules

        public static void Rule(AdditionalStatementElements lhs, Comma comma, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            lhs.CodeCompoundTerms.Add(statementElement.CodeCompoundTerm);
            lhs.CodeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);
        }

        public static void Rule(AdditionalStatementElements lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeCompoundTerm> CodeCompoundTerms
        {
            get { return m_codeCompoundTerms; }
        }

        #endregion
    }
}
