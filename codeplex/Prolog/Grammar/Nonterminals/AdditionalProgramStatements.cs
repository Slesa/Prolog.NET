/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Lingua;
using Prolog.Code;

namespace Prolog.Grammar
{
    // AdditionalProgramStatements ::= ; OptionalProgramStatement AdditionalProgramStatements
    //                             ::= nil
    //
    internal sealed class AdditionalProgramStatements : PrologNonterminal
    {
        #region Fields

        private List<CodeSentence> m_codeSentences = new List<CodeSentence>();

        #endregion

        #region Rules

        public static void Rule(AdditionalProgramStatements lhs, Period period, OptionalProgramStatement optionalProgramStatement, AdditionalProgramStatements additionalProgramStatements)
        {
            if (optionalProgramStatement.CodeSentence != null)
            {
                lhs.CodeSentences.Add(optionalProgramStatement.CodeSentence);
            }
            lhs.CodeSentences.AddRange(additionalProgramStatements.CodeSentences);
        }

        public static void Rule(AdditionalProgramStatements lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeSentence> CodeSentences
        {
            get { return m_codeSentences; }
        }

        #endregion
    }
}
