/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListBody ::= ListItems OptionalListTail
    //
    internal sealed class ListBody : PrologNonterminal
    {
        #region Fields

        private CodeList m_codeList;

        #endregion

        #region Rules

        public static void Rule(ListBody lhs, ListItems listItems, OptionalListTail optionalListTail)
        {
            CodeTerm tail;
            if (optionalListTail.CodeTerm != null)
            {
                tail = optionalListTail.CodeTerm;
            }
            else
            {
                tail = new CodeCompoundTerm(CodeFunctor.NilFunctor);
            }

            lhs.CodeList = new CodeList(listItems.CodeTerms, tail);
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
