/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;

namespace Prolog
{
    /// <summary>
    /// Represents a runtime call stack entry of a <see cref="PrologMachine"/>.
    /// </summary>
    public sealed class PrologStackFrame : IPrologInstructionStreamContainer, IPrologVariableListContainer, INotifyPropertyChanged
    {
        #region Fields

        private PrologStackFrameList m_container;
        private int m_stackIndex;

        private PrologInstructionStream m_instructionStream;
        private PrologVariableList m_variables;
        private Clause m_clause;

        private PrologInstruction m_currentInstruction;

        #endregion

        #region Constructors

        internal PrologStackFrame(PrologStackFrameList container, int stackIndex)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (stackIndex < 0)
            {
                throw new ArgumentOutOfRangeException("stackIndex");
            }

            m_container = container;
            m_stackIndex = stackIndex;

            m_instructionStream = null;
            m_variables = null;
            m_clause = null;
        }

        #endregion

        #region Public Properties

        public PrologStackFrameList Container
        {
            get { return m_container; }
        }

        public int StackIndex
        {
            get { return m_stackIndex; }
        }

        public PrologInstructionStream InstructionStream
        {
            get
            {
                if (m_instructionStream == null)
                {
                    m_instructionStream = new PrologInstructionStream(this);
                }

                return m_instructionStream;
            }
        }

        public PrologVariableList Variables
        {
            get
            {
                if (m_variables == null)
                {
                    m_variables = new PrologVariableList(this);
                }

                return m_variables;
            }
        }

        public Clause Clause
        {
            get
            {
                if (m_clause == null)
                {
                    WamInstructionStream wamInstructionStream = InstructionStream.WamInstructionStream;
                    foreach (WamInstructionStreamAttribute attribute in wamInstructionStream.Attributes)
                    {
                        WamInstructionStreamClauseAttribute clauseAttribute = attribute as WamInstructionStreamClauseAttribute;
                        if (clauseAttribute != null)
                        {
                            m_clause = Container.Machine.Program.Procedures[clauseAttribute.Functor].Clauses[clauseAttribute.Index];
                            break;
                        }
                    }
                }

                return m_clause;
            }
        }

        public PrologInstruction CurrentInstruction
        {
            get { return m_currentInstruction; }
            private set
            {
                if (value != m_currentInstruction)
                {
                    m_currentInstruction = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CurrentInstruction"));
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            if (Clause != null)
            {
                return string.Format("{0} : {1}", Clause.Container.Procedure.Functor, Clause.Index);
            }

            return "<Anonymous>";
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IPrologInstructionStreamContainer Members

        WamInstructionStream IPrologInstructionStreamContainer.WamInstructionStream
        {
            get
            {
                return Container.Machine.WamMachine.GetInstructionPointer(StackIndex).InstructionStream;
            }
        }

        #endregion

        #region Internal Members

        internal void Synchronize()
        {
            WamInstructionPointer wamInstructionPointer = Container.Machine.WamMachine.GetInstructionPointer(StackIndex);

            PrologInstruction currentInstruction = InstructionStream[wamInstructionPointer.Index];
            if (CurrentInstruction != currentInstruction)
            {
                if (CurrentInstruction != null)
                {
                    CurrentInstruction.IsCurrentInstruction = false;
                }
                CurrentInstruction = currentInstruction;
                if (CurrentInstruction != null)
                {
                    CurrentInstruction.IsCurrentInstruction = true;
                }
            }
        }

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
