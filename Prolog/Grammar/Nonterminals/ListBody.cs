/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListBody ::= ListItems OptionalListTail
    //
    internal sealed class ListBody : PrologNonterminal
    {
        public static void Rule(ListBody lhs, ListItems listItems, OptionalListTail optionalListTail)
        {
            var tail = optionalListTail.CodeTerm ?? new CodeCompoundTerm(CodeFunctor.NilFunctor);
            lhs.CodeList = new CodeList(listItems.CodeTerms, tail);
        }

        public CodeList CodeList { get; private set; }
    }
}
