/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalListBody ::= ListBody
    //                  ::= nil
    //
    internal sealed class OptionalListBody : PrologNonterminal
    {
        #region Fields

        private CodeList m_codeList;

        #endregion

        #region Rules

        public static void Rule(OptionalListBody lhs, ListBody listBody)
        {
            lhs.CodeList = listBody.CodeList;
        }

        public static void Rule(OptionalListBody lhs)
        { }

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
