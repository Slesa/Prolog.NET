/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // List ::= [ OptionalListBody ]
    //
    internal sealed class List : PrologNonterminal
    {
        public static void Rule(List lhs, OpenBracket openBracket, OptionalListBody optionalListBody, CloseBracket closeBracket)
        {
            lhs.CodeList = optionalListBody.CodeList ?? new CodeList();
        }

        public CodeList CodeList { get; private set; }
    }
}
