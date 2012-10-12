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
        #region Fields

        private CodeFunctor m_codeFunctor;

        #endregion

        #region Rules

        public static void Rule(BinaryOp200 lhs, OpPower op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp200 lhs, OpBitwiseExclusiveOr op)
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
