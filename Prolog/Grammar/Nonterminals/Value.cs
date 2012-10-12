/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Diagnostics;

using Prolog.Code;

namespace Prolog.Grammar
{
    // Value ::= integer
    //       ::= string
    //       ::= boolean
    //       ::= List
    //
    internal sealed class Value : PrologNonterminal
    {
        #region Fields

        private CodeTerm m_codeTerm;

        #endregion

        #region Rules

        public static void Rule(Value lhs, LiteralInteger literal)
        {
            CodeValueInteger codeValueInteger = new CodeValueInteger(int.Parse(literal.Text));

            lhs.CodeTerm = codeValueInteger;
        }

        public static void Rule(Value lhs, LiteralDouble literal)
        {
            CodeValueDouble codeValueDouble = new CodeValueDouble(double.Parse(literal.Text));

            lhs.CodeTerm = codeValueDouble;
        }

        public static void Rule(Value lhs, LiteralString literal)
        {
            Debug.Assert(!string.IsNullOrEmpty(literal.Text));
            Debug.Assert(literal.Text.Length >= 2);

            string unescapedText = literal.Text;
            unescapedText = unescapedText.Substring(1, unescapedText.Length - 2);
            unescapedText = unescapedText.Replace(@"""""", @"""");

            CodeValueString codeValueString = new CodeValueString(unescapedText);

            lhs.CodeTerm = codeValueString;
        }

        public static void Rule(Value lhs, LiteralBoolean literal)
        {
            CodeValueBoolean codeValueBoolean = new CodeValueBoolean(bool.Parse(literal.Text));

            lhs.CodeTerm = codeValueBoolean;
        }

        public static void Rule(Value lhs, List list)
        {
            lhs.CodeTerm = list.CodeList;
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
