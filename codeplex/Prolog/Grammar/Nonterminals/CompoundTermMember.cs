/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Diagnostics;

using Prolog.Code;

namespace Prolog.Grammar
{
    // CompoundTermMember ::= BinaryElementExpression700
    //
    internal sealed class CompoundTermMember : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(CompoundTermMember lhs, BinaryElementExpression700 binaryElementExpression)
        {
            lhs.CodeTerm = binaryElementExpression.CodeTerm;
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
