/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal class WamContext
    {
        private WamMachineStates m_state;
        private WamInstructionPointer m_instructionPointer;
        private IEnumerator<bool> m_predicateEnumerator;
        private WamInstructionPointer m_returnInstructionPointer;
        private int m_stackIndex;

        private WamEnvironment m_environment;
        private WamChoicePoint m_choicePoint;
        private WamReferenceTargetList m_argumentRegisters;
        private WamReferenceTargetList m_temporaryRegisters;
        private WamChoicePoint m_cutChoicePoint;

        private WamCompoundTerm m_currentStructure;
        private int m_currentStructureIndex;
        private UnifyModes m_currentUnifyMode;

        public WamContext()
        {
            m_argumentRegisters = new WamReferenceTargetList();
            m_temporaryRegisters = new WamReferenceTargetList();
        }

        public WamMachineStates State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        public WamInstructionPointer InstructionPointer
        {
            get { return m_instructionPointer; }
            set { m_instructionPointer = value; }
        }

        public IEnumerator<bool> PredicateEnumerator
        {
            get { return m_predicateEnumerator; }
            set { m_predicateEnumerator = value; }
        }

        public WamInstructionPointer ReturnInstructionPointer
        {
            get { return m_returnInstructionPointer; }
            set { m_returnInstructionPointer = value; }
        }

        public int StackIndex
        {
            get { return m_stackIndex; }
            set { m_stackIndex = value; }
        }

        public WamReferenceTargetList ArgumentRegisters
        {
            get { return m_argumentRegisters; }
        }

        public WamReferenceTargetList TemporaryRegisters
        {
            get { return m_temporaryRegisters; }
        }

        public WamChoicePoint ChoicePoint
        {
            get { return m_choicePoint; }
            set { m_choicePoint = value; }
        }

        public WamEnvironment Environment
        {
            get { return m_environment; }
            set { m_environment = value; }
        }

        public WamCompoundTerm CurrentStructure
        {
            get { return m_currentStructure; }
            set { m_currentStructure = value; }
        }

        public WamChoicePoint CutChoicePoint
        {
            get { return m_cutChoicePoint; }
            set { m_cutChoicePoint = value; }
        }

        public int CurrentStructureIndex
        {
            get { return m_currentStructureIndex; }
            set { m_currentStructureIndex = value; }
        }

        public UnifyModes CurrentUnifyMode
        {
            get { return m_currentUnifyMode; }
            set { m_currentUnifyMode = value; }
        }
    }
}
