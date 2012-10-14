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
        public static void Rule(OptionalRuleBody lhs, ColonDash colonDash, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            lhs.CodeCompoundTerms = new List<CodeCompoundTerm>();
            lhs.CodeCompoundTerms.Add(statementElement.CodeCompoundTerm);
            lhs.CodeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);
        }

        public static void Rule(OptionalRuleBody lhs)
        { }

        public List<CodeCompoundTerm> CodeCompoundTerms { get; private set; }
    }
}
