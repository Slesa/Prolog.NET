/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // List ::= [ OptionalListBody ]
    //
    internal sealed class List : PrologNonterminal
    {
        #region Fields

        private CodeList m_codeList;

        #endregion

        #region Rules

        public static void Rule(List lhs, OpenBracket openBracket, OptionalListBody optionalListBody, CloseBracket closeBracket)
        {
            if (optionalListBody.CodeList == null)
            {
                lhs.CodeList = new CodeList();
            }
            else
            {
                lhs.CodeList = optionalListBody.CodeList;
            }
        }

        #endregion

        #region Public Properties

        public CodeList CodeList
        {
            get { return m_codeList; }
            private set { m_codeList = value; }
        }

        #endregion
    }
}
