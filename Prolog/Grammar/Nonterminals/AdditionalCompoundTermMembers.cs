/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using Prolog.Code;

namespace Prolog.Grammar
{
    // AdditionalCompoundTermMembers ::= , CompoundTermMember AdditionalCompoundTermMembers
    //                               ::= nil;
    //
    internal sealed class AdditionalCompoundTermMembers : PrologNonterminal
    {
        readonly List<CodeTerm> _codeTerms = new List<CodeTerm>();

        public static void Rule(AdditionalCompoundTermMembers lhs, Comma comma, CompoundTermMember compoundTermMember, AdditionalCompoundTermMembers additionalCompoundTermMembers)
        {
            lhs.CodeTerms.Add(compoundTermMember.CodeTerm);
            lhs.CodeTerms.AddRange(additionalCompoundTermMembers.CodeTerms);
        }

        public static void Rule(AdditionalCompoundTermMembers lhs)
        { }

        public List<CodeTerm> CodeTerms
        {
            get { return _codeTerms; }
        }
    }
}
