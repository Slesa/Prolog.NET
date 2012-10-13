/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // Statement ::= Clause
    //           ::= Query
    //
    internal sealed class Statement : PrologNonterminal
    {
        public static void Rule(Statement lhs, Clause clause)
        {
            lhs.CodeSentence = clause.CodeSentence;
        }

        public static void Rule(Statement lhs, Query query)
        {
            lhs.CodeSentence = query.CodeSentence;
        }

        public CodeSentence CodeSentence { get; private set; }
    }
}
