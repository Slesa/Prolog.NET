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
        PrologInstructionStream _instructionStream;
        PrologVariableList _variables;
        Clause _clause;
        PrologInstruction _currentInstruction;

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

            Container = container;
            StackIndex = stackIndex;

            _instructionStream = null;
            _variables = null;
            _clause = null;
        }

        public PrologStackFrameList Container { get; private set; }

        public int StackIndex { get; private set; }

        public PrologInstructionStream InstructionStream
        {
            get { return _instructionStream ?? (_instructionStream = new PrologInstructionStream(this)); }
        }

        public PrologVariableList Variables
        {
            get { return _variables ?? (_variables = new PrologVariableList(this)); }
        }

        public Clause Clause
        {
            get
            {
                if (_clause == null)
                {
                    var wamInstructionStream = InstructionStream.WamInstructionStream;
                    foreach (var attribute in wamInstructionStream.Attributes)
                    {
                        var clauseAttribute = attribute as WamInstructionStreamClauseAttribute;
                        if (clauseAttribute != null)
                        {
                            _clause = Container.Machine.Program.Procedures[clauseAttribute.Functor].Clauses[clauseAttribute.Index];
                            break;
                        }
                    }
                }
                return _clause;
            }
        }

        public PrologInstruction CurrentInstruction
        {
            get { return _currentInstruction; }
            private set
            {
                if (value != _currentInstruction)
                {
                    _currentInstruction = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("CurrentInstruction"));
                }
            }
        }

        public override string ToString()
        {
            return Clause != null ? string.Format("{0} : {1}", Clause.Container.Procedure.Functor, Clause.Index) : "<Anonymous>";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        WamInstructionStream IPrologInstructionStreamContainer.WamInstructionStream
        {
            get
            {
                return Container.Machine.WamMachine.GetInstructionPointer(StackIndex).InstructionStream;
            }
        }

        internal void Synchronize()
        {
            var wamInstructionPointer = Container.Machine.WamMachine.GetInstructionPointer(StackIndex);
            var currentInstruction = InstructionStream[wamInstructionPointer.Index];
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

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
