/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal sealed class WamInstructionStreamClauseAttribute : WamInstructionStreamAttribute, IImmuttable
    {
        #region Fields

        private Functor m_functor;
        private int m_clauseIndex;

        #endregion

        #region Constructors

        public WamInstructionStreamClauseAttribute(int index, Functor functor, int clauseIndex)
            : base(index)
        {
            m_functor = functor;
            m_clauseIndex = clauseIndex;
        }

        #endregion

        #region Fields

        public Functor Functor
        {
            get { return m_functor; }
        }

        public int ClauseIndex
        {
            get { return m_clauseIndex; }
        }

        #endregion
    }
}
