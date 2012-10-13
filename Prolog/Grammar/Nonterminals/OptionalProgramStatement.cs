/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalProgramStatement ::= Statement
    //                          ::= nil
    //
    internal sealed class OptionalProgramStatement : PrologNonterminal
    {
        public static void Rule(OptionalProgramStatement lhs, Statement statement)
        {
            lhs.CodeSentence = statement.CodeSentence;
        }

        public static void Rule(OptionalProgramStatement lhs)
        { }

        public CodeSentence CodeSentence { get; private set; }
    }
}
