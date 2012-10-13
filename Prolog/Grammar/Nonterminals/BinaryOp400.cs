/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryOp400 ::= OpMultiply      *
    //             ::= OpDivide        /
    //             ::= OpRemainder     rem
    //             ::= OpModulo        mod
    //             ::= OpShiftLeft     <<
    //             ::= OpShiftRight    >>
    //
    internal sealed class BinaryOp400 : PrologNonterminal
    {
        public static void Rule(BinaryOp400 lhs, OpMultiply op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp400 lhs, OpDivide op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp400 lhs, OpRemainder op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp400 lhs, OpModulo op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp400 lhs, OpShiftLeft op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp400 lhs, OpShiftRight op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public CodeFunctor CodeFunctor { get; private set; }
    }
}
