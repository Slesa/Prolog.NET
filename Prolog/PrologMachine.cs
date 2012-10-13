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
        private WamMachine m_wamMachine;

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

            StackFrames = new PrologStackFrameList(this);
            Arguments = new PrologVariableList(this);
            TemporaryVariables = new PrologVariableList(this);

            Synchronize();

            QueryResults = null;
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

        public event EventHandler ExecutionSuspended;
        public event EventHandler<PrologQueryEventArgs> ExecutionComplete;

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

        public PrologStackFrameList StackFrames { get; private set; }
        public PrologVariableList Arguments { get; private set; }
        public PrologVariableList TemporaryVariables { get; private set; }
        public PrologQueryResults QueryResults { get; private set; }

        public PerformanceStatistics PerformanceStatistics
        {
            get { return WamMachine.PerformanceStatistics; }
        }

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
            var results = WamMachine.RunToBacktrack();
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
            var results = WamMachine.RunToSuccess();
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
            var results = WamMachine.StepIn();
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
            var results = WamMachine.StepOut();
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
            var results = WamMachine.StepOver();
            ProcessResults(results);
            return results;
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        internal WamMachine WamMachine
        {
            get { return m_wamMachine; }
        }

        bool IsBreakpoint(WamInstructionPointer wamInstructionPointer)
        {
            //HACK: Implement
            return false;
        }

        void ProcessResults(ExecutionResults results)
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

        void UpdateQueryResults()
        {
            var queryResults = new PrologQueryResults();
            queryResults.Variables.Synchronize(GetPermanentVariables(0, true));
            QueryResults = queryResults;
        }

        void RaiseExecutionSuspended()
        {
            if (ExecutionSuspended != null)
            {
                ExecutionSuspended(this, EventArgs.Empty);
            }
        }

        void RaiseExecutionComplete(PrologQueryEventArgs e)
        {
            if (ExecutionComplete != null)
            {
                ExecutionComplete(this, e);
            }
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        void Synchronize()
        {
            SynchronizeStackFrames();
            SynchronizeVariables();
        }

        void SynchronizeStackFrames()
        {
            var size = WamMachine.StackIndex + 1;

            // Remove obsolete entries from stack.
            //
            while (StackFrames.Count > size)
            {
                StackFrames.Pop();
            }

            // Purge stack frames that do not match their WAM machine counterparts.
            //
            for (var index = 0; index < Math.Min(size, StackFrames.Count); ++index)
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

        void SynchronizeVariables()
        {
            TemporaryVariables.Synchronize(GetTemporaryVariables());

            Arguments.Synchronize(GetArgumentVariables());

            for (int stackIndex = 0; stackIndex <= WamMachine.StackIndex; ++stackIndex)
            {
                var stackFrame = StackFrames[stackIndex];
                stackFrame.Variables.Synchronize(GetPermanentVariables(stackIndex, false));
            }
        }

        PrologVariableList GetTemporaryVariables()
        {
            var result = new PrologVariableList();

            for (var index = 0; index < WamMachine.TemporaryRegisters.Count; ++index)
            {
                var value = "*";
                var referenceTarget = WamMachine.TemporaryRegisters[index];
                if (referenceTarget != null)
                {
                    value = referenceTarget.ToString();
                }
                result.Add(string.Format("X{0}", index)).Text = value;
            }
            return result;
        }

        PrologVariableList GetArgumentVariables()
        {
            var result = new PrologVariableList();

            for (var index = 0; index < WamMachine.ArgumentRegisters.Count; ++index)
            {
                string value = "*";
                var referenceTarget = WamMachine.ArgumentRegisters[index];
                if (referenceTarget != null)
                {
                    value = referenceTarget.ToString();
                }
                result.Add(string.Format("A{0}", index)).Text = value;
            }

            return result;
        }

        PrologVariableList GetPermanentVariables(int stackIndex, bool getCodeTerm)
        {
            var result = new PrologVariableList();
            var environment = WamMachine.GetEnvironment(stackIndex);
            if (environment != null)
            {
                // Retrieve register name assignments from instruction stream.
                //
                var wamInstructionStream = WamMachine.GetInstructionPointer(stackIndex).InstructionStream;
                var variableNames = wamInstructionStream != null ? wamInstructionStream.GetPermanentVariableAssignments() : new Dictionary<int, string>();

                for (var index = 0; index < environment.PermanentRegisters.Count; ++index)
                {
                    var variable = result.Add(string.Format("Y{0}", index));

                    string name;
                    if (variableNames.TryGetValue(index, out name))
                    {
                        variable.Name = name;
                    }

                    var referenceTarget = environment.PermanentRegisters[index];
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
    }
}
