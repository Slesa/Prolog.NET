/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamChoicePoint
    {
        #region Fields

        public static int s_nextGeneration;

        private int m_generation;
        private WamChoicePoint m_predecessor;
        private WamEnvironment m_environment;
        private int m_stackIndex;
        private WamInstructionPointer m_returnInstructionPointer;
        private WamReferenceTargetList m_argumentRegisters;
        private WamChoicePoint m_cutChoicePoint;

        private WamInstructionPointer m_backtrackInstructionPointer;
        private IEnumerator<bool> m_predicateEnumerator;

        private List<WamVariable> m_trail;

        #endregion

        #region Constructors

        public WamChoicePoint(WamChoicePoint predecessor, WamEnvironment environment, int stackIndex, WamInstructionPointer returnInstructionPointer, IEnumerable<WamReferenceTarget> argumentRegisters, WamChoicePoint cutChoicePoint)
        {
            m_generation = s_nextGeneration++;

            m_predecessor = predecessor;
            m_environment = environment;
            m_stackIndex = stackIndex;
            m_returnInstructionPointer = returnInstructionPointer;
            m_argumentRegisters = new WamReferenceTargetList(argumentRegisters);
            m_cutChoicePoint = cutChoicePoint;

            m_backtrackInstructionPointer = WamInstructionPointer.Undefined;
            m_predicateEnumerator = null;

            m_trail = new List<WamVariable>();
        }

        #endregion

        #region Public Properties

        public int Generation
        {
            get { return m_generation; }
        }

        public WamChoicePoint Predecessor
        {
            get { return m_predecessor; }
        }

        public WamEnvironment Environment
        {
            get { return m_environment; }
        }

        public int StackIndex
        {
            get { return m_stackIndex; }
        }

        public WamInstructionPointer ReturnInstructionPointer
        {
            get { return m_returnInstructionPointer; }
        }

        public WamReferenceTargetList ArgumentRegisters
        {
            get { return m_argumentRegisters; }
        }

        public WamChoicePoint CutChoicePoint
        {
            get { return m_cutChoicePoint; }
        }

        public WamInstructionPointer BacktrackInstructionPointer
        {
            get { return m_backtrackInstructionPointer; }
            internal set { m_backtrackInstructionPointer = value; }
        }

        public IEnumerator<bool> PredicateEnumerator
        {
            get { return m_predicateEnumerator; }
            internal set { m_predicateEnumerator = value; }
        }

        public IList<WamVariable> Trail
        {
            get { return m_trail; }
        }

        #endregion
    }
}
