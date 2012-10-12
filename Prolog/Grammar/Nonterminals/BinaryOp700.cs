/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // BinaryOp700 ::= OpUnify            =
    //             ::= OpCanUnify         ?=
    //             ::= OpCannotUnify      \=
    //             ::= OpComposedOf       =..
    //             ::= OpTermEqual        ==
    //             ::= OpTermUnequal      \==
    //             ::= OpTermLess         @<
    //             ::= OpTermLessEqual    @=<
    //             ::= OpTermGreater      @>
    //             ::= OpTermGreaterEqual @>=
    //             ::= OpIs               is
    //             ::= OpEqual            =:=
    //             ::= OpUnqual           =\=
    //             ::= OpLess             <
    //             ::= OpLessEqual        =<
    //             ::= OpGreater          >
    //             ::= OpGreaterEqual     >=
    //
    internal sealed class BinaryOp700 : PrologNonterminal
    {
        #region Fields

        private CodeFunctor m_codeFunctor;

        #endregion

        #region Rules

        public static void Rule(BinaryOp700 lhs, OpUnify op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpCanUnify op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpCannotUnify op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpComposedOf op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermEqual op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermUnequal op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermLess op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermLessEqual op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermGreater op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpTermGreaterEqual op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpIs1 op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpIs2 op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpEqual op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpUnequal op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpLess op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpLessEqual op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpGreater op)
        {
            lhs.CodeFunctor = new CodeFunctor(op.Text, 2, true);
        }

        public static void Rule(BinaryOp700 lhs, OpGreaterEqual op)
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
