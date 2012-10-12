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
        #region Fields

        private List<CodeTerm> m_codeTerms = new List<CodeTerm>();

        #endregion

        #region Rules

        public static void Rule(AdditionalListItems lhs, Comma comma, ListItem listItem, AdditionalListItems additionalListItems)
        {
            lhs.CodeTerms.Add(listItem.CodeTerm);
            lhs.CodeTerms.AddRange(additionalListItems.CodeTerms);
        }

        public static void Rule(AdditionalListItems lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeTerm> CodeTerms
        {
            get { return m_codeTerms; }
        }

        #endregion
    }
}
