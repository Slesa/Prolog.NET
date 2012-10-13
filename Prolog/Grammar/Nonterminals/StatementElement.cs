/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog.Grammar
{
    // StatementElement ::= BinaryElementExpression700
    //                      Cut
    //
    internal sealed class StatementElement : PrologNonterminal
    {
        public static void Rule(StatementElement lhs, BinaryElementExpression700 binaryTermExpression)
        {
            var codeCompoundTerm =  binaryTermExpression.CodeTerm as CodeCompoundTerm;
            if (codeCompoundTerm == null)
            {
                throw new InvalidOperationException("Non-term expression specified.");
            }

            lhs.CodeCompoundTerm = codeCompoundTerm;
        }

        public static void Rule(StatementElement lhs, Cut cut)
        {
            lhs.CodeCompoundTerm = new CodeCompoundTerm(CodeFunctor.CutFunctor);
        }

        public CodeCompoundTerm CodeCompoundTerm { get; private set; }
    }
}
