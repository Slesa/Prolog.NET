/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryElementExpression700 ::= BinaryElementExpression700 BinaryOp700 BinaryElementExpression500
    //                            ::= BinaryElementExpression500
    //
    internal sealed class BinaryElementExpression700 : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(BinaryElementExpression700 lhs, BinaryElementExpression700 binaryElementExpression700, BinaryOp700 binaryOp700, BinaryElementExpression500 binaryElementExpression500)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    binaryOp700.CodeFunctor,
                    new CodeTerm[] { binaryElementExpression700.CodeTerm, binaryElementExpression500.CodeTerm });
        }

        public static void Rule(BinaryElementExpression700 lhs, BinaryElementExpression500 binaryElementExpression500)
        {
            lhs.CodeTerm = binaryElementExpression500.CodeTerm;
        }

        #endregion

        #region Public Properties

        public CodeTerm CodeTerm
        {
            get { return m_codeTerm; }
            private set { m_codeTerm = value; }
        }

        #endregion
    }
}
