/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // CompoundTermMember ::= BinaryElementExpression700
    //
    internal sealed class CompoundTermMember : PrologNonterminal
    {
        public static void Rule(CompoundTermMember lhs, BinaryElementExpression700 binaryElementExpression)
        {
            lhs.CodeTerm = binaryElementExpression.CodeTerm;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
