/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListTail ::= | ListTailItem
    //
    internal sealed class ListTail : PrologNonterminal
    {
        public static void Rule(ListTail lhs, Bar bar, ListTailItem listTailItem)
        {
            lhs.CodeTerm = listTailItem.CodeTerm;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
