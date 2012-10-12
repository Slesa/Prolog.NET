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
        #region Fields

        private CodeCompoundTerm m_codeCompoundTerm;

        #endregion

        #region Rules

        public static void Rule(Term lhs, Atom atom, OptionalTermBody optionalTermBody)
        {
            CodeCompoundTerm codeCompoundTerm;

            if (optionalTermBody.CodeTerms == null)
            {
                CodeFunctor codeFunctor = new CodeFunctor(atom.Text);

                codeCompoundTerm = new CodeCompoundTerm(codeFunctor);
            }
            else
            {
                CodeFunctor codeFunctor = new CodeFunctor(atom.Text, optionalTermBody.CodeTerms.Count);

                if (codeFunctor.Arity == 0)
                {
                    codeCompoundTerm = new CodeCompoundTerm(codeFunctor);
                }
                else
                {
                    codeCompoundTerm = new CodeCompoundTerm(codeFunctor, optionalTermBody.CodeTerms);
                }
            }

            lhs.CodeCompoundTerm = codeCompoundTerm;
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
