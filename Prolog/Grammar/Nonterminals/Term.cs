/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog.Grammar
{
    // Term ::= atom OptionalTermBody
    //
    internal sealed class Term : PrologNonterminal
    {
        public static void Rule(Term lhs, Atom atom, OptionalTermBody optionalTermBody)
        {
            CodeCompoundTerm codeCompoundTerm;

            if (optionalTermBody.CodeTerms == null)
            {
                var codeFunctor = new CodeFunctor(atom.Text);
                codeCompoundTerm = new CodeCompoundTerm(codeFunctor);
            }
            else
            {
                var codeFunctor = new CodeFunctor(atom.Text, optionalTermBody.CodeTerms.Count);
                codeCompoundTerm = codeFunctor.Arity == 0 ? new CodeCompoundTerm(codeFunctor) : new CodeCompoundTerm(codeFunctor, optionalTermBody.CodeTerms);
            }

            lhs.CodeCompoundTerm = codeCompoundTerm;
        }

        public CodeCompoundTerm CodeCompoundTerm { get; private set; }
    }
}
