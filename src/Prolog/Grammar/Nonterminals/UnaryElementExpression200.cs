/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // UnaryElementExpression200 ::= UnaryOp200 Element
    //                           ::= Element
    //
    internal sealed class UnaryElementExpression200 : PrologNonterminal
    {
        public static void Rule(UnaryElementExpression200 lhs, UnaryOp200 unaryOp200, Element element)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    unaryOp200.CodeFunctor,
                    new[] { element.CodeTerm });
        }

        public static void Rule(UnaryElementExpression200 lhs, Element element)
        {
            lhs.CodeTerm = element.CodeTerm;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
