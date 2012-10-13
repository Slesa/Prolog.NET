/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryElementExpression400 ::= BinaryElementExpression400 BinaryOp400 BinaryElementExpression200
    //                            ::= BinaryElementExpression200
    //
    internal sealed class BinaryElementExpression400 : PrologNonterminal
    {
        public static void Rule(BinaryElementExpression400 lhs, BinaryElementExpression400 binaryElementExpression400, BinaryOp400 binaryOp400, BinaryElementExpression200 binaryElementExpression200)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    binaryOp400.CodeFunctor,
                    new[] { binaryElementExpression400.CodeTerm, binaryElementExpression200.CodeTerm });
        }

        public static void Rule(BinaryElementExpression400 lhs, BinaryElementExpression200 binaryElementExpression200)
        {
            lhs.CodeTerm = binaryElementExpression200.CodeTerm;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
