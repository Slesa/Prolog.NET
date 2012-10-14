/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListItem ::= CompoundTermMember
    //
    internal sealed class ListItem : PrologNonterminal
    {
        public static void Rule(ListItem lhs, CompoundTermMember compoundTermMember)
        {
            lhs.CodeTerm = compoundTermMember.CodeTerm;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
