/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Lingua;
using Prolog.Code;

namespace Prolog.Grammar
{
    // Program ::= OptionalProgramStatement AdditionalProgramStatements
    //
    [Nonterminal(IsStart = true)]
    internal sealed class Program : PrologNonterminal
    {
        #region Fields

        private List<CodeSentence> m_codeSentences = new List<CodeSentence>();

        #endregion

        #region Rules

        public static void Rule(Program lhs, OptionalProgramStatement optionalProgramStatement, AdditionalProgramStatements additionalProgramStatements)
        {
            if (optionalProgramStatement.CodeSentence != null)
            {
                lhs.CodeSentences.Add(optionalProgramStatement.CodeSentence);
            }
            lhs.CodeSentences.AddRange(additionalProgramStatements.CodeSentences);
        }

        #endregion

        #region Public Properties

        public List<CodeSentence> CodeSentences
        {
            get { return m_codeSentences; }
        }

        #endregion
    }
}
