/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // UnaryOp200 ::= OpNegate        -
    //            ::= OpBitwiseNegate \
    //
    internal sealed class UnaryOp200 : PrologNonterminal
    {
        public static void Rule(UnaryOp200 lhs, OpSubtract op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 1, true);
        }

        public static void Rule(UnaryOp200 lhs, OpBitwiseNegate op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 1, true);
        }

        public CodeFunctor CodeFunctor { get; private set; }
    }
}
