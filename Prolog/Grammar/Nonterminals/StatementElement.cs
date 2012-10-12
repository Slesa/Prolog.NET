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
        #region Fields

        private CodeCompoundTerm m_codeCompoundTerm;

        #endregion

        #region Rules

        public static void Rule(StatementElement lhs, BinaryElementExpression700 binaryTermExpression)
        {
            CodeCompoundTerm codeCompoundTerm =  binaryTermExpression.CodeTerm as CodeCompoundTerm;
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

        #endregion

        #region Public Properties

        public CodeCompoundTerm CodeCompoundTerm
        {
            get { return m_codeCompoundTerm; }
            private set { m_codeCompoundTerm = value; }
        }

        #endregion
    }
}
