/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using Prolog.Code;

namespace Prolog.Grammar
{
    // AdditionalListItems ::= , ListItem AdditionalListItems
    //                     ::= nil;
    //
    internal sealed class AdditionalListItems : PrologNonterminal
    {
        readonly List<CodeTerm> _codeTerms = new List<CodeTerm>();

        public static void Rule(AdditionalListItems lhs, Comma comma, ListItem listItem, AdditionalListItems additionalListItems)
        {
            lhs.CodeTerms.Add(listItem.CodeTerm);
            lhs.CodeTerms.AddRange(additionalListItems.CodeTerms);
        }

        public static void Rule(AdditionalListItems lhs)
        { }

        public List<CodeTerm> CodeTerms
        {
            get { return _codeTerms; }
        }
    }
}
