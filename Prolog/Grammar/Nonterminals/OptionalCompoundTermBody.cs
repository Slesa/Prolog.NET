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
        public static void Rule(OptionalCompoundTermBody lhs, CompoundTermBody compoundTermBody)
        {
            lhs.CodeTerms = new List<CodeTerm>();
            lhs.CodeTerms.AddRange(compoundTermBody.CodeTerms);
        }

        public static void Rule(OptionalCompoundTermBody lhs)
        { }

        public List<CodeTerm> CodeTerms { get; private set; }
    }
}
