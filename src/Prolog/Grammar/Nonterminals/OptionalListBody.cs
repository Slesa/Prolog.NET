/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalListBody ::= ListBody
    //                  ::= nil
    //
    internal sealed class OptionalListBody : PrologNonterminal
    {
        public static void Rule(OptionalListBody lhs, ListBody listBody)
        {
            lhs.CodeList = listBody.CodeList;
        }

        public static void Rule(OptionalListBody lhs)
        { }

        public CodeList CodeList { get; private set; }
    }
}
