/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalRuleBody ::= :- StatementElement AdditionalStatementElements
    //                  ::= nil
    //
    internal sealed class OptionalRuleBody : PrologNonterminal
    {
        #region Fields

        List<CodeCompoundTerm> m_codeCompoundTerms;

        #endregion

        #region Rules

        public static void Rule(OptionalRuleBody lhs, ColonDash colonDash, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            lhs.CodeCompoundTerms = new List<CodeCompoundTerm>();
            lhs.CodeCompoundTerms.Add(statementElement.CodeCompoundTerm);
            lhs.CodeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);
        }

        public static void Rule(OptionalRuleBody lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeCompoundTerm> CodeCompoundTerms
        {
            get { return m_codeCompoundTerms; }
            private set { m_codeCompoundTerms = value; }
        }

        #endregion
    }
}
