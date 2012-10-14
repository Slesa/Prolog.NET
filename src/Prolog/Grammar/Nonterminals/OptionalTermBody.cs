/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalTermBody ::= ( OptionalCompoundTermBody )
    //                  ::= nil
    //
    internal sealed class OptionalTermBody : PrologNonterminal
    {
        public static void Rule(OptionalTermBody lhs, OpenParenthesis openParenthesis, OptionalCompoundTermBody optionalCompoundTermBody, CloseParenthesis closeParenthesis)
        {
            lhs.CodeTerms = new List<CodeTerm>();
            if (optionalCompoundTermBody.CodeTerms != null)
            {
                lhs.CodeTerms.AddRange(optionalCompoundTermBody.CodeTerms);
            }
        }

        public static void Rule(OptionalTermBody lhs)
        { }

        public List<CodeTerm> CodeTerms { get; private set; }
    }
}
