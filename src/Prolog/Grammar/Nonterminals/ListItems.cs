/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListItems ::= ListItem AdditionalListItems
    //
    internal sealed class ListItems : PrologNonterminal
    {
        readonly List<CodeTerm> _codeTerms = new List<CodeTerm>();

        public static void Rule(ListItems lhs, ListItem listItem, AdditionalListItems additionalListItems)
        {
            lhs.CodeTerms.Add(listItem.CodeTerm);
            lhs.CodeTerms.AddRange(additionalListItems.CodeTerms);
        }

        public List<CodeTerm> CodeTerms
        {
            get { return _codeTerms; }
        }
    }
}
