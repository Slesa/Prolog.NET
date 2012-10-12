/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal sealed class WamEnvironment
    {
        #region Fields

        private static int s_nextId;

        private int m_id;
        private WamEnvironment m_predecessor;
        private WamInstructionPointer m_returnInstructionPointer;
        private WamChoicePoint m_cutChoicePoint;

        private WamReferenceTargetList m_permanentRegisters;

        #endregion

        #region Constructors

        public WamEnvironment(WamEnvironment predecessor, WamInstructionPointer returnInstructionPointer, WamChoicePoint cutChoicePoint)
        {
            m_id = s_nextId++;
            m_predecessor = predecessor;
            m_returnInstructionPointer = returnInstructionPointer;
            m_cutChoicePoint = cutChoicePoint;

            m_permanentRegisters = new WamReferenceTargetList();
        }

        #endregion

        #region Public Properties

        public int Id
        {
            get { return m_id; }
        }

        public WamEnvironment Predecessor
        {
            get { return m_predecessor; }
        }

        public WamInstructionPointer ReturnInstructionPointer
        {
            get { return m_returnInstructionPointer; }
        }

        public WamReferenceTargetList PermanentRegisters
        {
            get { return m_permanentRegisters; }
        }

        public WamChoicePoint CutChoicePoint
        {
            get { return m_cutChoicePoint; }
        }

        #endregion
    }
}
