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
        public static void Rule(Query lhs, ColonDash colonDash, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            var codeCompoundTerms = new List<CodeCompoundTerm> {statementElement.CodeCompoundTerm};
            codeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);

            lhs.CodeSentence = new CodeSentence(null, null, codeCompoundTerms);
        }

        public CodeSentence CodeSentence { get; private set; }
    }
}
