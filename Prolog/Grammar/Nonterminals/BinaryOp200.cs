/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryOp200 ::= OpPower              **
    //             ::= OpBitwiseExclusiveOr ^
    //
    internal sealed class BinaryOp200 : PrologNonterminal
    {
        public static void Rule(BinaryOp200 lhs, OpPower op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp200 lhs, OpBitwiseExclusiveOr op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public CodeFunctor CodeFunctor { get; private set; }
    }
}
