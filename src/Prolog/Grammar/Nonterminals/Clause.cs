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
        public static void Rule(Clause lhs, OptionalProcedureComments optionalProcedureComments, Term term, OptionalRuleBody optionalBody)
        {
            var comments = optionalProcedureComments.Comments ??
                           (IEnumerable<CodeComment>) new CodeComment[] {};

            var codeCompoundTerms = optionalBody.CodeCompoundTerms ??
                                    (IEnumerable<CodeCompoundTerm>) new CodeCompoundTerm[] {};

            lhs.CodeSentence = new CodeSentence(comments, term.CodeCompoundTerm, codeCompoundTerms);
        }

        public CodeSentence CodeSentence { get; private set; }
    }
}
