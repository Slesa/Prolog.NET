/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using Prolog.Code;

namespace Prolog.Grammar
{
    // AdditionalProgramStatements ::= ; OptionalProgramStatement AdditionalProgramStatements
    //                             ::= nil
    //
    internal sealed class AdditionalProgramStatements : PrologNonterminal
    {
        readonly List<CodeSentence> _codeSentences = new List<CodeSentence>();

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

        public List<CodeSentence> CodeSentences
        {
            get { return _codeSentences; }
        }
    }
}
