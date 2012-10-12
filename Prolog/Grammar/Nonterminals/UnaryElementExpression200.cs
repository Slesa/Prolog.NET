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
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(UnaryElementExpression200 lhs, UnaryOp200 unaryOp200, Element element)
        {
            lhs.CodeTerm =
                new CodeCompoundTerm(
                    unaryOp200.CodeFunctor,
                    new CodeTerm[] { element.CodeTerm });
        }

        public static void Rule(UnaryElementExpression200 lhs, Element element)
        {
            lhs.CodeTerm = element.CodeTerm;
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
