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
        readonly List<CodeCompoundTerm> _codeCompoundTerms = new List<CodeCompoundTerm>();

        public static void Rule(AdditionalStatementElements lhs, Comma comma, StatementElement statementElement, AdditionalStatementElements additionalStatementElements)
        {
            lhs.CodeCompoundTerms.Add(statementElement.CodeCompoundTerm);
            lhs.CodeCompoundTerms.AddRange(additionalStatementElements.CodeCompoundTerms);
        }

        public static void Rule(AdditionalStatementElements lhs)
        { }

        public List<CodeCompoundTerm> CodeCompoundTerms
        {
            get { return _codeCompoundTerms; }
        }
    }
}
