/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal sealed class WamInstructionStreamClauseAttribute : WamInstructionStreamAttribute, IImmuttable
    {
        public WamInstructionStreamClauseAttribute(int index, Functor functor, int clauseIndex)
            : base(index)
        {
            Functor = functor;
            ClauseIndex = clauseIndex;
        }

        public Functor Functor { get; private set; }
        public int ClauseIndex { get; private set; }
    }
}
