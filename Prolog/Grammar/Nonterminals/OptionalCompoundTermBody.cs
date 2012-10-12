/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalCompoundTermBody ::= CompoundTermBody 
    //                          ::= nil
    //
    internal sealed class OptionalCompoundTermBody : PrologNonterminal
    {
        #region Fields

        private List<CodeTerm> m_codeTerms;

        #endregion

        #region Rules

        public static void Rule(OptionalCompoundTermBody lhs, CompoundTermBody compoundTermBody)
        {
            lhs.CodeTerms = new List<CodeTerm>();
            lhs.CodeTerms.AddRange(compoundTermBody.CodeTerms);
        }

        public static void Rule(OptionalCompoundTermBody lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeTerm> CodeTerms
        {
            get { return m_codeTerms; }
            private set { m_codeTerms = value; }
        }

        #endregion
    }
}
