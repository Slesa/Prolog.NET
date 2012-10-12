/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryOp500 ::= OpAdd        +
    //             ::= OpSubtract   -
    //             ::= OpBitwiseAnd /\
    //             ::= OpBitwiseOr  \/
    //
    internal sealed class BinaryOp500 : PrologNonterminal
    {
        #region Fields

        private CodeFunctor m_codeFunctor;

        #endregion

        #region Rules

        public static void Rule(BinaryOp500 lhs, OpAdd op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp500 lhs, OpSubtract op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp500 lhs, OpBitwiseAnd op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp500 lhs, OpBitwiseOr op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        #endregion

        #region Public Properties

        public CodeFunctor CodeFunctor
        {
            get { return m_codeFunctor; }
            private set { m_codeFunctor = value; }
        }

        #endregion
    }
}
