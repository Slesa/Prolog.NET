/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalListTail ::= ListTail
    //                  ::= nil
    //
    internal sealed class OptionalListTail : PrologNonterminal
    {
        public static void Rule(OptionalListTail lhs, ListTail listTail)
        {
            lhs.CodeTerm = listTail.CodeTerm;
        }

        public static void Rule(OptionalListTail lhs)
        { }

        public CodeTerm CodeTerm { get; private set; }
    }
}
