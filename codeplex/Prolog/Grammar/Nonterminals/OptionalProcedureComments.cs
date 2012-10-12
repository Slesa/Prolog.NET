/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog.Grammar
{
    // OptionalProcedureComments ::= procedureComment OptionalProcedureComments
    //                           ::= nil
    //
    internal sealed class OptionalProcedureComments : PrologNonterminal
    {
        #region Fields

        private List<CodeComment> m_comments;

        #endregion

        #region Rules

        public static void Rule(OptionalProcedureComments lhs, ProcedureComment procedureComment, OptionalProcedureComments optionalProcedureComments)
        {
            string comment = procedureComment.Text;

            Debug.Assert(comment.StartsWith("///"));

            comment = comment.Substring(3).Trim();

            lhs.Comments = new List<CodeComment>();
            lhs.Comments.Add(new CodeComment(comment));
            if (optionalProcedureComments.Comments != null)
            {
                lhs.Comments.AddRange(optionalProcedureComments.Comments);
            }
        }

        public static void Rule(OptionalProcedureComments lhs)
        { }

        #endregion

        #region Public Properties

        public List<CodeComment> Comments
        {
            get { return m_comments; }
            private set { m_comments = value; }
        }

        #endregion
    }
}
