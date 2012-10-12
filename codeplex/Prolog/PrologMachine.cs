/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Prolog
{
    /// <summary>
    /// Defines an agent capable of evaluating Prolog queries. 
    /// </summary>
    /// <remarks>
    /// Encapsulates a <see cref="WamMachine"/>.
    /// </remarks>
    public sealed class PrologMachine : IPrologVariableListContainer, INotifyPropertyChanged
    {
        #region Fields

        private WamMachine m_wamMachine;

        private PrologStackFrameList m_stackFrames;
        private PrologVariableList m_arguments;
        private PrologVariableList m_temporaryVariables;

        private PrologQueryResults m_queryResults;

        #endregion

        #region Constructors

        private PrologMachine(Program program, Query query)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            m_wamMachine = WamMachine.Create(program, query);

            m_stackFrames = new PrologStackFrameList(this);
            m_arguments = new PrologVariableList(this);
            m_temporaryVariables = new PrologVariableList(this);

            Synchronize();

            m_queryResults = null;
        }

        public static PrologMachine Create(Program program, Query query)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return new PrologMachine(program, query);
        }

        #endregion

        #region Events

        public event EventHandler ExecutionSuspended;
        public event EventHandler<PrologQueryEventArgs> ExecutionComplete;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="Prolog.Program"/> used to evaluate the <see cref="Query"/>.
        /// </summary>
        public Program Program
        {
            get { return WamMachine.Program; }
        }

        /// <summary>
        /// Gets the <see cref="Prolog.Query"/> to evaluate.
        /// </summary>
        public Query Query
        {
            get { return WamMachine.Query; }
        }

        public PrologStackFrameList StackFrames
        {
            get { return m_stackFrames; }
        }

        public PrologVariableList Arguments
        {
            get { return m_arguments; }
        }

        public PrologVariableList TemporaryVariables
        {
            get { return m_temporaryVariables; }
        }

        public PrologQueryResults QueryResults
        {
            get { return m_queryResults; }
            private set { m_queryResults = value; }
        }

        public PerformanceStatistics PerformanceStatistics
        {
            get { return WamMachine.PerformanceStatistics; }
        }

        #endregion

        #region Public Methods: Run

        public bool CanEndProgram
        {
            get { return true; }
        }

        public void EndProgram()
        { }

        public bool CanRestart
        {
            get { return true; }
        }

        public void Restart()
        {
            WamMachine.Initialize();

            Synchronize();

            QueryResults = null;
        }

        public bool CanRunToBacktrack
        {
            get
            {
                return WamMachine.State != WamMachineStates.Halt;
            }
        }

        public ExecutionResults RunToBacktrack()
        {
            ExecutionResults results = WamMachine.RunToBacktrack();

            ProcessResults(results);

            return results;
        }

        public bool CanRunToSuccess
        {
            get
            {
                return WamMachine.State != WamMachineStates.Halt;
            }
        }

        public ExecutionResults RunToSuccess()
        {
            ExecutionResults results = WamMachine.RunToSuccess();

            ProcessResults(results);

            return results;
        }

        public bool CanStepIn
        {
            get
            {
                return WamMachine.State != WamMachineStates.Halt;
            }
        }

        public ExecutionResults StepIn()
        {
            ExecutionResults results = WamMachine.StepIn();

            ProcessResults(results);

            return results;
        }

        public bool CanStepOut
        {
            get
            {
                return WamMachine.State != WamMachineStates.Halt;
            }
        }

        public ExecutionResults StepOut()
        {
            ExecutionResults results = WamMachine.StepOut();

            ProcessResults(results);

            return results;
        }

        public bool CanStepOver
        {
            get
            {
                return WamMachine.State != WamMachineStates.Halt;
            }
        }

        public ExecutionResults StepOver()
        {
            ExecutionResults results = WamMachine.StepOver();

            ProcessResults(results);

            return results;
        }

        #endregion

        #region Public Methods: Breakpoint

        public bool CanAddBreakpoint
        {
            get { return false; }
        }

        public void AddBreakpoint()
        { }

        public bool CanClearAllBreakpoints
        {
            get { return false; }
        }

        public void ClearAllBreakpoints()
        { }

        public bool CanClearBreakpoint
        {
            get { return false; }
        }

        public void ClearBreakpoint()
        { }

        public bool CanToggleBreakpoint
        {
            get { return false; }
        }

        public void ToggleBreakpoint()
        { }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Internal Members

        internal WamMachine WamMachine
        {
            get { return m_wamMachine; }
        }

        #endregion

        #region Hidden Members

        private bool IsBreakpoint(WamInstructionPointer wamInstructionPointer)
        {
            //HACK: Implement
            return false;
        }

        private void ProcessResults(ExecutionResults results)
        {
            Synchronize();

            switch (results)
            {
                case ExecutionResults.Success:
                    {
                        UpdateQueryResults();
                        RaiseExecutionComplete(new PrologQueryEventArgs(QueryResults));
                    }
                    break;

                case ExecutionResults.Failure:
                    {
                        RaiseExecutionComplete(new PrologQueryEventArgs(null));
                    }
                    break;

                case ExecutionResults.None:
                case ExecutionResults.Backtrack:
                    {
                        RaiseExecutionSuspended();
                    }
                    break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown results {0}.", results));
            }
        }

        private void UpdateQueryResults()
        {
            PrologQueryResults queryResults = new PrologQueryResults();

            queryResults.Variables.Synchronize(GetPermanentVariables(0, true));

            QueryResults = queryResults;
        }

        private void RaiseExecutionSuspended()
        {
            if (ExecutionSuspended != null)
            {
                ExecutionSuspended(this, EventArgs.Empty);
            }
        }

        private void RaiseExecutionComplete(PrologQueryEventArgs e)
        {
            if (ExecutionComplete != null)
            {
                ExecutionComplete(this, e);
            }
        }

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion

        #region Hidden Members: SynchronizeState

        private void Synchronize()
        {
            SynchronizeStackFrames();
            SynchronizeVariables();
        }

        private void SynchronizeStackFrames()
        {
            int size = WamMachine.StackIndex + 1;

            // Remove obsolete entries from stack.
            //
            while (StackFrames.Count > size)
            {
                StackFrames.Pop();
            }

            // Purge stack frames that do not match their WAM machine counterparts.
            //
            for (int index = 0; index < Math.Min(size, StackFrames.Count); ++index)
            {
                if (StackFrames[index].InstructionStream.WamInstructionStream == WamMachine.GetInstructionPointer(index).InstructionStream)
                {
                    StackFrames[index].Synchronize();
                }
                else
                {
                    while (StackFrames.Count >= (index + 1))
                    {
                        StackFrames.Pop();
                    }
                    break;
                }
            }

            // Add new entries to stack.
            //
            while (StackFrames.Count < size)
            {
                StackFrames.Push().Synchronize();
            }
        }

        private void SynchronizeVariables()
        {
            TemporaryVariables.Synchronize(GetTemporaryVariables());

            Arguments.Synchronize(GetArgumentVariables());

            for (int stackIndex = 0; stackIndex <= WamMachine.StackIndex; ++stackIndex)
            {
                PrologStackFrame stackFrame = StackFrames[stackIndex];
                stackFrame.Variables.Synchronize(GetPermanentVariables(stackIndex, false));
            }
        }

        private PrologVariableList GetTemporaryVariables()
        {
            PrologVariableList result = new PrologVariableList();

            for (int index = 0; index < WamMachine.TemporaryRegisters.Count; ++index)
            {
                string value = "*";
                WamReferenceTarget referenceTarget = WamMachine.TemporaryRegisters[index];
                if (referenceTarget != null)
                {
                    value = referenceTarget.ToString();
                }

                result.Add(string.Format("X{0}", index)).Text = value;
            }

            return result;
        }

        private PrologVariableList GetArgumentVariables()
        {
            PrologVariableList result = new PrologVariableList();

            for (int index = 0; index < WamMachine.ArgumentRegisters.Count; ++index)
            {
                string value = "*";
                WamReferenceTarget referenceTarget = WamMachine.ArgumentRegisters[index];
                if (referenceTarget != null)
                {
                    value = referenceTarget.ToString();
                }

                result.Add(string.Format("A{0}", index)).Text = value;
            }

            return result;
        }

        private PrologVariableList GetPermanentVariables(int stackIndex, bool getCodeTerm)
        {
            PrologVariableList result = new PrologVariableList();

            WamEnvironment environment = WamMachine.GetEnvironment(stackIndex);
            if (environment != null)
            {
                // Retrieve register name assignments from instruction stream.
                //
                Dictionary<int, string> variableNames;
                WamInstructionStream wamInstructionStream = WamMachine.GetInstructionPointer(stackIndex).InstructionStream;
                if (wamInstructionStream != null)
                {
                    variableNames = wamInstructionStream.GetPermanentVariableAssignments();
                }
                else
                {
                    variableNames = new Dictionary<int, string>();
                }

                for (int index = 0; index < environment.PermanentRegisters.Count; ++index)
                {
                    PrologVariable variable = result.Add(string.Format("Y{0}", index));

                    string name;
                    if (variableNames.TryGetValue(index, out name))
                    {
                        variable.Name = name;
                    }

                    WamReferenceTarget referenceTarget = environment.PermanentRegisters[index];
                    if (referenceTarget != null)
                    {
                        if (getCodeTerm)
                        {
                            variable.CodeTerm = referenceTarget.GetCodeTerm();
                        }
                        else
                        {
                            variable.Text = referenceTarget.ToString();
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
