/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryElementExpression200 ::= BinaryElementExpression200 BinaryOp200 UnaryElementExpression200
    //                            ::= UnaryElementExpression200
    //
    internal sealed class BinaryElementExpression200 : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(BinaryElementExpression200 lhs, BinaryElementExpression200 binaryElementExpression200, BinaryOp200 binaryOp200, UnaryElementExpression200 unaryElementExpression200)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    binaryOp200.CodeFunctor,
                    new CodeTerm[] { binaryElementExpression200.CodeTerm, unaryElementExpression200.CodeTerm });
        }

        public static void Rule(BinaryElementExpression200 lhs, UnaryElementExpression200 unaryElementExpression200)
        {
            lhs.CodeTerm = unaryElementExpression200.CodeTerm;
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
