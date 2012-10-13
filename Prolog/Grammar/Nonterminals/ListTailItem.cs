/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // ListTailItem ::= variable
    //              ::= List
    //
    internal sealed class ListTailItem : PrologNonterminal
    {
        public static void Rule(ListTailItem lhs, Variable variable)
        {
            var codeVariable = new CodeVariable(variable.Text);
            lhs.CodeTerm = codeVariable;
        }

        public static void Rule(ListTailItem lhs, List list)
        {
            lhs.CodeTerm = list.CodeList;
        }

        public CodeTerm CodeTerm { get; private set; }
    }
}
