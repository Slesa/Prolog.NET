/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryElementExpression500 ::= BinaryElementExpression500 BinaryOp500 BinaryElementExpression400
    //                            ::= BinaryElementExpression400
    //
    internal sealed class BinaryElementExpression500 : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(BinaryElementExpression500 lhs, BinaryElementExpression500 binaryElementExpression500, BinaryOp500 binaryOp500, BinaryElementExpression400 binaryElementExpression400)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    binaryOp500.CodeFunctor,
                    new CodeTerm[] { binaryElementExpression500.CodeTerm, binaryElementExpression400.CodeTerm });
        }

        public static void Rule(BinaryElementExpression500 lhs, BinaryElementExpression400 binaryElementExpression400)
        {
            lhs.CodeTerm = binaryElementExpression400.CodeTerm;
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
