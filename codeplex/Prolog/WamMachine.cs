/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal sealed class WamMachine
    {
        #region Fields

        private const int DEFAULT_STACK_SIZE_LIMIT = 50000;
        private const int MAX_STACK_SIZE_LIMIT = 1000000;

        private Program m_program;
        private Query m_query;

        private int m_stackSizeLimit = DEFAULT_STACK_SIZE_LIMIT;

        private Stack<WamContext> m_contextStack;
        private WamContext m_currentContext;

        private PerformanceStatistics m_performanceStatistics;

        #endregion

        #region Constructors

        private WamMachine(Program program, Query query)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            m_program = program;
            m_query = query;

            m_contextStack = new Stack<WamContext>();
            m_currentContext = null;

            m_performanceStatistics = new PerformanceStatistics();
        }

        public static WamMachine Create(Program program, Query query)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            WamMachine wamMachine = new WamMachine(program, query);

            wamMachine.Initialize();

            return wamMachine;
        }

        #endregion

        #region Events

        public event EventHandler<WamMachineStepEventArgs> Stepped;

        #endregion

        #region Public Properties

        public Program Program
        {
            get { return m_program; }
        }

        public Query Query
        {
            get { return m_query; }
        }

        public WamMachineStates State
        {
            get { return CurrentContext.State; }
            private set { CurrentContext.State = value; }
        }

        public int StackIndex
        {
            get { return CurrentContext.StackIndex; }
            private set { CurrentContext.StackIndex = value; }
        }

        public WamEnvironment Environment
        {
            get { return CurrentContext.Environment; }
            private set { CurrentContext.Environment = value; }
        }

        public WamReferenceTargetList ArgumentRegisters
        {
            get { return CurrentContext.ArgumentRegisters; }
        }

        public WamReferenceTargetList TemporaryRegisters
        {
            get { return CurrentContext.TemporaryRegisters; }
        }

        public PerformanceStatistics PerformanceStatistics
        {
            get { return m_performanceStatistics; }
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            ClearContextStack();
            PushContext(Query.WamInstructionStream);
        }

        public ExecutionResults RunToBacktrack()
        {
            if (State == WamMachineStates.Halt)
            {
                return ExecutionResults.Failure;
            }

            PerformanceStatistics.Start();

            WamMachineStepEventArgsState stepEventArgsState = new WamMachineStepEventArgsState();
            WamMachineStepEventArgs stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            ExecutionResults results = ExecutionResults.None;

            bool loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure
                    || results == ExecutionResults.Success
                    || results == ExecutionResults.Backtrack)
                {
                    loop = false;
                }
                else
                {
                    stepEventArgsState.InstructionPointer = InstructionPointer;
                    RaiseStepped(stepEventArgs);
                    if (stepEventArgs.Breakpoint)
                    {
                        loop = false;
                    }
                }
            }

            PerformanceStatistics.Stop();

            return results;
        }

        public ExecutionResults RunToSuccess()
        {
            if (State == WamMachineStates.Halt)
            {
                return ExecutionResults.Failure;
            }

            PerformanceStatistics.Start();

            WamMachineStepEventArgsState stepEventArgsState = new WamMachineStepEventArgsState();
            WamMachineStepEventArgs stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            ExecutionResults results = ExecutionResults.None;

            bool loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure
                    || results == ExecutionResults.Success)
                {
                    loop = false;
                }
                else
                {
                    stepEventArgsState.InstructionPointer = InstructionPointer;
                    RaiseStepped(stepEventArgs);
                    if (stepEventArgs.Breakpoint)
                    {
                        loop = false;
                    }
                }
            }

            PerformanceStatistics.Stop();

            return results;
        }

        public ExecutionResults StepIn()
        {
            if (State == WamMachineStates.Halt)
            {
                return ExecutionResults.Failure;
            }

            PerformanceStatistics.Start();

            ExecutionResults results = Step();

            PerformanceStatistics.Stop();

            return results;
        }

        public ExecutionResults StepOut()
        {
            if (State == WamMachineStates.Halt)
            {
                return ExecutionResults.Failure;
            }

            PerformanceStatistics.Start();

            WamMachineStepEventArgsState stepEventArgsState = new WamMachineStepEventArgsState();
            WamMachineStepEventArgs stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            ExecutionResults results = ExecutionResults.None;

            int initialStackIndex = StackIndex;
            bool loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure
                    || results == ExecutionResults.Success)
                {
                    loop = false;
                }
                else if (StackIndex < initialStackIndex)
                {
                    loop = false;
                }
                else
                {
                    stepEventArgsState.InstructionPointer = InstructionPointer;
                    RaiseStepped(stepEventArgs);
                    if (stepEventArgs.Breakpoint)
                    {
                        loop = false;
                    }
                }
            }

            PerformanceStatistics.Stop();

            return results;
        }

        public ExecutionResults StepOver()
        {
            if (State == WamMachineStates.Halt)
            {
                return ExecutionResults.Failure;
            }

            PerformanceStatistics.Start();

            WamMachineStepEventArgsState stepEventArgsState = new WamMachineStepEventArgsState();
            WamMachineStepEventArgs stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            ExecutionResults results = ExecutionResults.None;

            int initialStackIndex = StackIndex;
            bool loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure
                    || results == ExecutionResults.Success)
                {
                    loop = false;
                }
                else if (StackIndex <= initialStackIndex)
                {
                    loop = false;
                }
                else
                {
                    stepEventArgsState.InstructionPointer = InstructionPointer;
                    RaiseStepped(stepEventArgs);
                    if (stepEventArgs.Breakpoint)
                    {
                        loop = false;
                    }
                }
            }

            PerformanceStatistics.Stop();

            return results;
        }

        public void SetInstructionStream(WamInstructionStream instructionStream)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }

            ReturnInstructionPointer = InstructionPointer.GetNext();
            CutChoicePoint = ChoicePoint;
            TemporaryRegisters.Clear();

            StackIndex += 1;
            InstructionPointer = new WamInstructionPointer(instructionStream);
        }

        public void PushContext(WamInstructionStream instructionStream)
        {
            if (instructionStream == null)
            {
                throw new ArgumentNullException("instructionStream");
            }

            WamContext context = new WamContext();
            m_contextStack.Push(context);
            m_currentContext = context;

            State = WamMachineStates.Run;
            InstructionPointer = new WamInstructionPointer(instructionStream);
            ReturnInstructionPointer = WamInstructionPointer.Undefined;
            StackIndex = 0;

            Environment = null;
            ChoicePoint = null;
            ArgumentRegisters.Clear();
            TemporaryRegisters.Clear();

            CurrentStructure = null;
            CurrentStructureIndex = -1;
        }

        public void PopContext(bool unwindTrail)
        {
            if (m_contextStack.Count == 0)
            {
                throw new InvalidOperationException("Context stack is empty.");
            }

            if (unwindTrail)
            {
                while (ChoicePoint != null)
                {
                    UnwindTrail();
                    ChoicePoint = ChoicePoint.Predecessor;
                }
            }

            m_contextStack.Pop();

            if (m_contextStack.Count == 0)
            {
                m_currentContext = null;
            }
            else
            {
                m_currentContext = m_contextStack.Peek();
            }
        }

        #endregion

        #region Internal Members

        internal WamEnvironment GetEnvironment(int stackIndex)
        {
            if (stackIndex < 0 || stackIndex > StackIndex)
            {
                throw new ArgumentOutOfRangeException("stackIndex");
            }

            // Determine the offset relative to the current stack index.  0 indicates
            // the current stack index, 1 indicates the prior stack index, and so on.
            //
            int delta = StackIndex - stackIndex;

            // Determine if we have yet to create an environment for this stack
            // frame.
            //
            if (ReturnInstructionPointer != WamInstructionPointer.Undefined)
            {
                if (delta == 0)
                {
                    // No environment has been created for the requested stack frame.
                    //
                    return null;
                }
                else
                {
                    // Adjust delta to account for the lack of a current stack frame.
                    //
                    delta -= 1;
                }
            }

            // Search for the requested environment.
            //
            WamEnvironment environment = Environment;
            if (environment == null)
            {
                if (stackIndex == 0)
                {
                    return null;
                }
                else
                {
                    throw new InvalidOperationException("Environment does not exist.");
                }
            }
            while (delta > 0)
            {
                environment = environment.Predecessor;
                if (environment == null)
                {
                    throw new InvalidOperationException("Environment does not exist.");
                }
                delta -= 1;
            }

            return environment;
        }

        internal WamInstructionPointer GetInstructionPointer(int stackIndex)
        {
            if (stackIndex < 0 || stackIndex > StackIndex)
            {
                throw new ArgumentOutOfRangeException("stackIndex");
            }

            // Determine the offset relative to the current stack index.  0 indicates
            // the current stack index, 1 indicates the prior stack index, and so on.
            //
            int delta = StackIndex - stackIndex;

            // If we've requested the instruction stream for the current stack index,
            // simply use the current instruction pointer.
            //
            if (delta == 0)
            {
                return InstructionPointer;
            }
            delta -= 1;

            // If we've requested the prior stack frame and we have not allocated a new 
            // environment, use the current return instruction.
            //
            if (ReturnInstructionPointer != WamInstructionPointer.Undefined)
            {
                if (delta == 0)
                {
                    return ReturnInstructionPointer.GetPrior();
                }
                delta -= 1;
            }

            // Use the environment chain to retrieve prior stack frames.  Note that since
            // we're using the return instruction pointer, the current environment
            // is used to retrieve the prior stack frame, and so on.
            //
            WamEnvironment environment = Environment;
            if (environment == null)
            {
                throw new InvalidOperationException("Environment does not exist.");
            }
            while (delta > 0)
            {
                environment = environment.Predecessor;
                if (environment == null)
                {
                    throw new InvalidOperationException("Environment does not exist.");
                }
                delta -= 1;
            }

            return environment.ReturnInstructionPointer.GetPrior();
        }

        internal bool Unify(WamReferenceTarget lhs, WamReferenceTarget rhs)
        {
            return Unify(lhs, rhs, false);
        }

        internal bool CanUnify(WamReferenceTarget lhs, WamReferenceTarget rhs)
        {
            return Unify(lhs, rhs, true);
        }

        internal bool CannotUnify(WamReferenceTarget lhs, WamReferenceTarget rhs)
        {
            return !Unify(lhs, rhs, true);
        }

        internal WamReferenceTarget Evaluate(WamReferenceTarget wamReferenceTarget)
        {
            if (wamReferenceTarget == null)
            {
                throw new ArgumentNullException("wamReferenceTarget");
            }

            WamCompoundTerm wamCompoundTerm = wamReferenceTarget.Dereference() as WamCompoundTerm;
            if (wamCompoundTerm == null)
            {
                return wamReferenceTarget;
            }

            if (!Program.Libraries.Contains(wamCompoundTerm.Functor))
            {
                return wamReferenceTarget;
            }

            LibraryMethod method = Program.Libraries[wamCompoundTerm.Functor];
            if (method == null
                || method.CanEvaluate == false)
            {
                return wamReferenceTarget;
            }

            WamReferenceTarget[] arguments = new WamReferenceTarget[method.Functor.Arity];
            for (int index = 0; index < method.Functor.Arity; ++index)
            {
                arguments[index] = Evaluate(wamCompoundTerm.Children[index]);
            }

            Function function = method as Function;
            if (function != null)
            {
                CodeTerm[] codeTerms = new CodeTerm[method.Functor.Arity];
                for (int index = 0; index < method.Functor.Arity; ++index)
                {
                    codeTerms[index] = arguments[index].GetCodeTerm();
                }

                CodeTerm result;
                try
                {
                    result = function.FunctionDelegate(codeTerms);
                    if (result == null)
                    {
                        result = new CodeValueObject(null);
                    }
                }
                catch (Exception ex)
                {
                    result = new CodeValueException(ex);
                }

                return WamValue.Create(result);
            }

            Predicate predicate = method as Predicate;
            if (predicate != null)
            {
                bool result;
                try
                {
                    result = predicate.PredicateDelegate(this, arguments);
                }
                catch
                {
                    result = false;
                }

                return WamValueBoolean.Create(result);
            }

            return wamReferenceTarget;
        }

        #endregion

        #region Hidden Members: Properties

        private int StackSizeLimit
        {
            get { return m_stackSizeLimit; }
            set
            {
                if (value < 0
                    || value > MAX_STACK_SIZE_LIMIT)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                m_stackSizeLimit = value;
            }
        }

        private WamContext CurrentContext
        {
            get { return m_currentContext; }
        }

        private WamInstructionPointer InstructionPointer
        {
            get { return CurrentContext.InstructionPointer; }
            set { CurrentContext.InstructionPointer = value; }
        }

        private IEnumerator<bool> PredicateEnumerator
        {
            get { return CurrentContext.PredicateEnumerator; }
            set { CurrentContext.PredicateEnumerator = value; }
        }

        private WamInstructionPointer ReturnInstructionPointer
        {
            get { return CurrentContext.ReturnInstructionPointer; }
            set { CurrentContext.ReturnInstructionPointer = value; }
        }

        private WamChoicePoint ChoicePoint
        {
            get { return CurrentContext.ChoicePoint; }
            set { CurrentContext.ChoicePoint = value; }
        }

        private WamChoicePoint CutChoicePoint
        {
            get { return CurrentContext.CutChoicePoint; }
            set { CurrentContext.CutChoicePoint = value; }
        }

        private WamCompoundTerm CurrentStructure
        {
            get { return CurrentContext.CurrentStructure; }
            set { CurrentContext.CurrentStructure = value; }
        }

        private int CurrentStructureIndex
        {
            get { return CurrentContext.CurrentStructureIndex; }
            set { CurrentContext.CurrentStructureIndex = value; }
        }

        private UnifyModes CurrentUnifyMode
        {
            get { return CurrentContext.CurrentUnifyMode; }
            set { CurrentContext.CurrentUnifyMode = value; }
        }

        private int Generation
        {
            get
            {
                if (ChoicePoint != null)
                {
                    return ChoicePoint.Generation;
                }

                return -1;
            }
        }

        #endregion

        #region Hidden Members

        private void ClearContextStack()
        {
            m_contextStack.Clear();
            m_currentContext = null;
        }

        private WamVariable CreateVariable()
        {
            return new WamVariable(Generation);
        }

        private WamCompoundTerm CreateCompoundTerm(Functor functor)
        {
            WamCompoundTerm value = WamCompoundTerm.Create(functor);

            CurrentStructure = value;
            CurrentStructureIndex = -1;

            return value;
        }

        private WamInstructionPointer GetInstructionPointer(Functor functor, int index)
        {
            Procedure procedure;
            if (m_program.Procedures.TryGetProcedure(functor, out procedure))
            {
                if (procedure.Clauses.Count >= index)
                {
                    return new WamInstructionPointer(procedure.Clauses[index].WamInstructionStream);
                }
                else
                {
                    return WamInstructionPointer.Undefined;
                }
            }
            else
            {
                return WamInstructionPointer.Undefined;
            }
        }

        private void SetRegister(WamInstructionRegister register, WamReferenceTarget value)
        {
            switch (register.Type)
            {
                case WamInstructionRegisterTypes.Argument: ArgumentRegisters[register.Id] = value; break;
                case WamInstructionRegisterTypes.Permanent: Environment.PermanentRegisters[register.Id] = value; break;
                case WamInstructionRegisterTypes.Temporary: TemporaryRegisters[register.Id] = value; break;

                default:
                    throw new InvalidOperationException(string.Format("Unknown register type {0}.", register.Type));
            }
        }

        private WamReferenceTarget GetRegister(WamInstructionRegister register)
        {
            switch (register.Type)
            {
                case WamInstructionRegisterTypes.Argument: return ArgumentRegisters[register.Id];
                case WamInstructionRegisterTypes.Permanent: return Environment.PermanentRegisters[register.Id];
                case WamInstructionRegisterTypes.Temporary: return TemporaryRegisters[register.Id];

                default:
                    throw new InvalidOperationException(string.Format("Unknown register type {0}.", register.Type));
            }
        }

        private WamReferenceTargetList GetPermanentVariables(int stackIndex)
        {
            WamEnvironment environment = GetEnvironment(stackIndex);
            if (environment == null)
            {
                return null;
            }

            return environment.PermanentRegisters;
        }

        private ExecutionResults Execute(WamInstruction instruction)
        {
            PerformanceStatistics.IncrementInstructionCount();

            switch (instruction.OpCode)
            {
                // Control Flow
                //
                case WamInstructionOpCodes.Allocate: return OnAllocate(instruction);
                case WamInstructionOpCodes.Call: return OnCall(instruction);
                case WamInstructionOpCodes.Cut: return OnCut(instruction);
                case WamInstructionOpCodes.Execute: return OnExecute(instruction);
                case WamInstructionOpCodes.Deallocate: return OnDeallocate(instruction);
                case WamInstructionOpCodes.LibraryCall: return OnLibraryCall(instruction);
                case WamInstructionOpCodes.Noop: return OnNoop(instruction);
                case WamInstructionOpCodes.Proceed: return OnProceed(instruction);
                case WamInstructionOpCodes.RetryMeElse: return OnRetryMeElse(instruction);
                case WamInstructionOpCodes.Success: return OnSuccess(instruction);
                case WamInstructionOpCodes.Failure: return OnFailure(instruction);
                case WamInstructionOpCodes.TrustMe: return OnTrustMe(instruction);
                case WamInstructionOpCodes.TryMeElse: return OnTryMeElse(instruction);

                // Put/Set
                //
                case WamInstructionOpCodes.PutBoundVariable: return OnPutBoundVariable(instruction);
                case WamInstructionOpCodes.PutStructure: return OnPutStructure(instruction);
                case WamInstructionOpCodes.PutUnboundVariable: return OnPutUnboundVariable(instruction);
                case WamInstructionOpCodes.PutValue: return OnPutValue(instruction);
                case WamInstructionOpCodes.SetBoundVariable: return OnSetBoundVariable(instruction);
                case WamInstructionOpCodes.SetUnboundVariable: return OnSetUnboundVariable(instruction);
                case WamInstructionOpCodes.SetValue: return OnSetValue(instruction);

                // Get/Unify
                //
                case WamInstructionOpCodes.GetBoundVariable: return OnGetBoundVariable(instruction);
                case WamInstructionOpCodes.GetStructure: return OnGetStructure(instruction);
                case WamInstructionOpCodes.GetUnboundVariable: return OnGetUnboundVariable(instruction);
                case WamInstructionOpCodes.GetValue: return OnGetValue(instruction);
                case WamInstructionOpCodes.UnifyBoundVariable: return OnUnifyBoundVariable(instruction);
                case WamInstructionOpCodes.UnifyUnboundVariable: return OnUnifyUnboundVariable(instruction);
                case WamInstructionOpCodes.UnifyValue: return OnUnifyValue(instruction);

                default:
                    throw new InvalidOperationException(string.Format("Unknown opcode {0}.", instruction.OpCode));
            }
        }

        private ExecutionResults Step()
        {
            switch (State)
            {
                case WamMachineStates.Backtrack:
                    if (Backtrack())
                    {
                        State = WamMachineStates.Run;
                        return ExecutionResults.None;
                    }
                    else
                    {
                        State = WamMachineStates.Halt;
                        return ExecutionResults.Failure;
                    }

                case WamMachineStates.Halt:
                    return ExecutionResults.Failure;

                case WamMachineStates.Run:

                    ExecutionResults results;
                    if (PredicateEnumerator != null)
                    {
                        if (PredicateEnumerator.MoveNext() == false)
                        {
                            ChoicePoint = ChoicePoint.Predecessor;

                            results = ExecutionResults.Backtrack;
                        }
                        else
                        {
                            results = ExecutionResults.None;
                        }
                        PredicateEnumerator = null;
                    }
                    else
                    {
                        WamInstruction instruction = InstructionPointer.Instruction;
                        results = Execute(instruction);
                    }

                    switch (results)
                    {
                        case ExecutionResults.None:
                            // No action required
                            break;

                        case ExecutionResults.Backtrack:
                            State = WamMachineStates.Backtrack;
                            break;

                        case ExecutionResults.Success:
                            State = WamMachineStates.Backtrack;
                            break;

                        case ExecutionResults.Failure:
                            State = WamMachineStates.Halt;
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unknown execution result {0}.", results));
                    }

                    return results;

                default:
                    throw new InvalidOperationException(string.Format("Unknown state {0}.", State));
            }
        }

        private void RaiseStepped(WamMachineStepEventArgs e)
        {
            if (Stepped != null)
            {
                Stepped(this, e);
            }
        }

        #endregion

        #region Hidden Members: Control Flow

        private ExecutionResults OnAllocate(WamInstruction instruction)
        {
            Environment = new WamEnvironment(Environment, ReturnInstructionPointer, CutChoicePoint);
            ReturnInstructionPointer = WamInstructionPointer.Undefined;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnCall(WamInstruction instruction)
        {
            WamInstructionPointer instructionPointer = GetInstructionPointer(instruction.Functor, 0);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }

            if (StackIndex >= StackSizeLimit)
            {
                return ExecutionResults.Failure;
            }

            ReturnInstructionPointer = InstructionPointer.GetNext();
            CutChoicePoint = ChoicePoint;
            TemporaryRegisters.Clear();

            StackIndex += 1;
            InstructionPointer = instructionPointer;

            return ExecutionResults.None;
        }

        private ExecutionResults OnCut(WamInstruction instruction)
        {
            while (ChoicePoint != CutChoicePoint)
            {
                ChoicePoint = ChoicePoint.Predecessor;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnExecute(WamInstruction instruction)
        {
            WamInstructionPointer instructionPointer = GetInstructionPointer(instruction.Functor, 0);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }

            if (StackIndex >= StackSizeLimit)
            {
                return ExecutionResults.Failure;
            }

            CutChoicePoint = ChoicePoint;
            TemporaryRegisters.Clear();

            InstructionPointer = instructionPointer;

            return ExecutionResults.None;
        }

        private ExecutionResults OnDeallocate(WamInstruction instruction)
        {
            if (Environment == null)
            {
                throw new InvalidOperationException("Environment not found.");
            }

            ReturnInstructionPointer = Environment.ReturnInstructionPointer;
            Environment = Environment.Predecessor;
            CutChoicePoint = Environment.CutChoicePoint;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnLibraryCall(WamInstruction instruction)
        {
            LibraryMethod method = Program.Libraries[instruction.Functor];

            Function function = method as Function;
            if (function != null)
            {
                return OnLibraryCallFunction(instruction, function);
            }

            Predicate predicate = method as Predicate;
            if (predicate != null)
            {
                return OnLibraryCallPredicate(instruction, predicate);
            }

            BacktrackingPredicate backtrackingPredicate = method as BacktrackingPredicate;
            if (backtrackingPredicate != null)
            {
                return OnLibraryCallBacktrackingPredicate(instruction, backtrackingPredicate);
            }

            CodePredicate codePredicate = method as CodePredicate;
            if (codePredicate != null)
            {
                return OnLibraryCallCodePredicate(instruction, codePredicate);
            }

            // Library method not found.
            //
            return ExecutionResults.Failure;
        }

        private ExecutionResults OnLibraryCallPredicate(WamInstruction instruction, Predicate predicate)
        {
            WamReferenceTarget[] arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (int index = 0; index < instruction.Functor.Arity; ++index)
            {
                arguments[index] = ArgumentRegisters[index];
            }

            bool result;
            try
            {
                result = predicate.PredicateDelegate(this, arguments);
            }
            catch
            {
                // Backtrack on exception.
                //
                return ExecutionResults.Backtrack;
            }

            if (result == false)
            {
                return ExecutionResults.Backtrack;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnLibraryCallCodePredicate(WamInstruction instruction, CodePredicate predicate)
        {
            WamReferenceTarget[] arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (int index = 0; index < instruction.Functor.Arity; ++index)
            {
                arguments[index] = ArgumentRegisters[index];
            }

            try
            {
                predicate.CodePredicateDelegate(this, arguments);
            }
            catch
            {
                // Backtrack on exception.
                //
                return ExecutionResults.Backtrack;
            }

            return ExecutionResults.None;
        }

        private ExecutionResults OnLibraryCallBacktrackingPredicate(WamInstruction instruction, BacktrackingPredicate predicate)
        {
            WamReferenceTarget[] arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (int index = 0; index < instruction.Functor.Arity; ++index)
            {
                arguments[index] = ArgumentRegisters[index];
            }

            IEnumerable<bool> enumerable;
            try
            {
                enumerable = predicate.BacktrackingPredicateDelegate(this, arguments);
            }
            catch
            {
                // Backtrack on exception.
                //
                return ExecutionResults.Backtrack;
            }

            IEnumerator<bool> enumerator = enumerable.GetEnumerator();

            ChoicePoint = new WamChoicePoint(ChoicePoint, Environment, StackIndex, ReturnInstructionPointer, ArgumentRegisters, CutChoicePoint);
            ChoicePoint.BacktrackInstructionPointer = InstructionPointer.GetNext();
            ChoicePoint.PredicateEnumerator = enumerator;

            InstructionPointer = ChoicePoint.BacktrackInstructionPointer;
            PredicateEnumerator = ChoicePoint.PredicateEnumerator;

            return ExecutionResults.None;
        }

        private ExecutionResults OnLibraryCallFunction(WamInstruction instruction, Function function)
        {
            CodeTerm[] functionArguments = new CodeTerm[instruction.Functor.Arity];
            for (int index = 0; index < instruction.Functor.Arity; ++index)
            {
                functionArguments[index] = Evaluate(ArgumentRegisters[index]).GetCodeTerm();
            }

            CodeTerm functionResult;
            try
            {
                functionResult = function.FunctionDelegate(functionArguments);
            }
            catch
            {
                // Backtrack on exception.
                //
                return ExecutionResults.Backtrack;
            }

            try
            {
                CodeValue functionResultValue = (CodeValue)functionResult;
                if (Convert.ToBoolean(functionResultValue.Object))
                {
                    InstructionPointer = InstructionPointer.GetNext();

                    return ExecutionResults.None;
                }
                else
                {
                    // Result converts to false.
                    //
                    return ExecutionResults.Backtrack;
                }
            }
            catch
            {
                // Result cannot be converted to a boolean.
                //
                return ExecutionResults.Backtrack;
            }
        }

        private ExecutionResults OnNoop(WamInstruction instruction)
        {
            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnProceed(WamInstruction instruction)
        {
            StackIndex -= 1;
            InstructionPointer = ReturnInstructionPointer;

            ReturnInstructionPointer = WamInstructionPointer.Undefined;
            TemporaryRegisters.Clear();

            return ExecutionResults.None;
        }

        private ExecutionResults OnRetryMeElse(WamInstruction instruction)
        {
            WamInstructionPointer instructionPointer = GetInstructionPointer(instruction.Functor, instruction.Index);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }

            ChoicePoint.BacktrackInstructionPointer = instructionPointer;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnSuccess(WamInstruction instruction)
        {
            return ExecutionResults.Success;
        }

        private ExecutionResults OnFailure(WamInstruction instruction)
        {
            return ExecutionResults.Failure;
        }

        private ExecutionResults OnTrustMe(WamInstruction instruction)
        {
            if (ChoicePoint == null)
            {
                throw new InvalidOperationException("Choice point not found.");
            }

            ChoicePoint = ChoicePoint.Predecessor;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnTryMeElse(WamInstruction instruction)
        {
            WamInstructionPointer instructionPointer = GetInstructionPointer(instruction.Functor, instruction.Index);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }

            ChoicePoint = new WamChoicePoint(ChoicePoint, Environment, StackIndex, ReturnInstructionPointer, ArgumentRegisters, CutChoicePoint);
            ChoicePoint.BacktrackInstructionPointer = instructionPointer;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        #endregion

        #region Hidden Members: Put/Set

        private ExecutionResults OnPutValue(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, instruction.ReferenceTarget);

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnPutStructure(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, CreateCompoundTerm(instruction.Functor));

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnPutBoundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnPutUnboundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.SourceRegister, CreateVariable());
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnSetBoundVariable(WamInstruction instruction)
        {
            if (CurrentStructure == null)
            {
                throw new InvalidOperationException("No current structure.");
            }

            CurrentStructureIndex += 1;
            if (CurrentStructureIndex >= CurrentStructure.Functor.Arity)
            {
                throw new InvalidOperationException("Current structure arity exceeded.");
            }

            CurrentStructure.Children[CurrentStructureIndex] = GetRegister(instruction.TargetRegister);

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnSetUnboundVariable(WamInstruction instruction)
        {
            if (CurrentStructure == null)
            {
                throw new InvalidOperationException("No current structure.");
            }

            CurrentStructureIndex += 1;
            if (CurrentStructureIndex >= CurrentStructure.Functor.Arity)
            {
                throw new InvalidOperationException("Current structure arity exceeded.");
            }

            SetRegister(instruction.TargetRegister, CreateVariable());
            CurrentStructure.Children[CurrentStructureIndex] = GetRegister(instruction.TargetRegister);

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnSetValue(WamInstruction instruction)
        {
            if (CurrentStructure == null)
            {
                throw new InvalidOperationException("No current structure.");
            }

            CurrentStructureIndex += 1;
            if (CurrentStructureIndex >= CurrentStructure.Functor.Arity)
            {
                throw new InvalidOperationException("Current structure arity exceeded.");
            }

            CurrentStructure.Children[CurrentStructureIndex] = instruction.ReferenceTarget;

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        #endregion

        #region Hidden Members: Get/Unify

        private ExecutionResults OnGetValue(WamInstruction instruction)
        {
            if (!(Unify(instruction.ReferenceTarget, GetRegister(instruction.SourceRegister))))
            {
                return ExecutionResults.Backtrack;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnGetStructure(WamInstruction instruction)
        {
            WamReferenceTarget sourceReference = GetRegister(instruction.SourceRegister).Dereference();
            WamVariable sourceVariable = sourceReference as WamVariable;
            WamCompoundTerm sourceCompoundTerm = sourceReference as WamCompoundTerm;

            // Ensure target is either a variable or compound term.
            //
            Debug.Assert(sourceVariable != null || sourceCompoundTerm != null);

            if (sourceVariable != null)
            {
                WamCompoundTerm compoundTerm = WamCompoundTerm.Create(instruction.Functor);
                Bind(sourceVariable, compoundTerm);

                CurrentStructure = compoundTerm;
                CurrentStructureIndex = -1;
                CurrentUnifyMode = UnifyModes.Write;
            }
            else // targetCompoundTerm != null
            {
                if (sourceCompoundTerm.Functor != instruction.Functor)
                {
                    return ExecutionResults.Backtrack;
                }

                CurrentStructure = sourceCompoundTerm;
                CurrentStructureIndex = -1;
                CurrentUnifyMode = UnifyModes.Read;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnGetBoundVariable(WamInstruction instruction)
        {
            if (!(Unify(GetRegister(instruction.TargetRegister), GetRegister(instruction.SourceRegister))))
            {
                return ExecutionResults.Backtrack;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnGetUnboundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnUnifyBoundVariable(WamInstruction instruction)
        {
            CurrentStructureIndex += 1;

            if (CurrentUnifyMode == UnifyModes.Read)
            {
                if (!Unify(GetRegister(instruction.TargetRegister), CurrentStructure.Children[CurrentStructureIndex]))
                {
                    return ExecutionResults.Backtrack;
                }
            }
            else // write
            {
                CurrentStructure.Children[CurrentStructureIndex] = GetRegister(instruction.TargetRegister);
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnUnifyValue(WamInstruction instruction)
        {
            CurrentStructureIndex += 1;

            if (CurrentUnifyMode == UnifyModes.Read)
            {
                if (!Unify(instruction.ReferenceTarget, CurrentStructure.Children[CurrentStructureIndex]))
                {
                    return ExecutionResults.Backtrack;
                }
            }
            else // write
            {
                CurrentStructure.Children[CurrentStructureIndex] = instruction.ReferenceTarget;
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        private ExecutionResults OnUnifyUnboundVariable(WamInstruction instruction)
        {
            CurrentStructureIndex += 1;

            if (CurrentUnifyMode == UnifyModes.Read)
            {
                SetRegister(instruction.TargetRegister, CurrentStructure.Children[CurrentStructureIndex]);
            }
            else // write
            {
                SetRegister(instruction.TargetRegister, CreateVariable());
                CurrentStructure.Children[CurrentStructureIndex] = GetRegister(instruction.TargetRegister);
            }

            InstructionPointer = InstructionPointer.GetNext();

            return ExecutionResults.None;
        }

        #endregion

        #region Hidden Members: Utility

        private void Bind(WamVariable variable, WamReferenceTarget target)
        {
            variable.Bind(target);
            Trail(variable);
        }

        private void Trail(WamVariable variable)
        {
            if (ChoicePoint != null)
            {
                if (variable.Generation <= ChoicePoint.Generation)
                {
                    ChoicePoint.Trail.Add(variable);
                }
            }
        }

        private void UnwindTrail()
        {
            if (ChoicePoint != null)
            {
                foreach (WamVariable variable in ChoicePoint.Trail)
                {
                    variable.Unbind();

                }

                ChoicePoint.Trail.Clear();
            }
        }

        private bool Backtrack()
        {
            if (State != WamMachineStates.Backtrack)
            {
                throw new InvalidOperationException("Backtrack not required.");
            }

            if (ChoicePoint == null)
            {
                return false;
            }

            UnwindTrail();

            Environment = ChoicePoint.Environment;
            StackIndex = ChoicePoint.StackIndex;
            ReturnInstructionPointer = ChoicePoint.ReturnInstructionPointer;
            ArgumentRegisters.Copy(ChoicePoint.ArgumentRegisters);
            CutChoicePoint = ChoicePoint.CutChoicePoint;

            InstructionPointer = ChoicePoint.BacktrackInstructionPointer;
            PredicateEnumerator = ChoicePoint.PredicateEnumerator;

            return true;
        }

        private bool Unify(WamReferenceTarget lhs, WamReferenceTarget rhs, bool testOnly)
        {
            if (lhs == null)
            {
                throw new ArgumentNullException("lhs");
            }
            if (rhs == null)
            {
                throw new ArgumentNullException("rhs");
            }

            lhs = lhs.Dereference();
            rhs = rhs.Dereference();

            WamVariable lhsVariable = lhs as WamVariable;
            WamVariable rhsVariable = rhs as WamVariable;

            WamCompoundTerm lhsCompoundTerm = lhs as WamCompoundTerm;
            WamCompoundTerm rhsCompoundTerm = rhs as WamCompoundTerm;

            WamValue lhsValue = lhs as WamValue;
            WamValue rhsValue = rhs as WamValue;

            // Ensure that each term is either a compound term or variable.
            //
            Debug.Assert(lhsVariable != null || lhsCompoundTerm != null || lhsValue != null);
            Debug.Assert(rhsVariable != null || rhsCompoundTerm != null || rhsValue != null);

            // Ensure if that we've dereferenced to a variable that it is unbound.
            //
            Debug.Assert(lhsVariable == null || lhsVariable.Target == null);
            Debug.Assert(rhsVariable == null || rhsVariable.Target == null);

            if (lhsVariable != null)
            {
                if (rhsVariable != null)
                {
                    if (lhsVariable.Generation < rhsVariable.Generation)
                    {
                        if (!testOnly) Bind(rhsVariable, lhs);
                    }
                    else
                    {
                        if (!testOnly) Bind(lhsVariable, rhs);
                    }
                }
                else
                {
                    if (!testOnly) Bind(lhsVariable, rhs);
                }
            }
            else
            {
                if (rhsVariable != null)
                {
                    if (!testOnly) Bind(rhsVariable, lhs);
                }
                else
                {
                    if (lhsCompoundTerm != null && rhsCompoundTerm != null)
                    {
                        if (lhsCompoundTerm.Functor != rhsCompoundTerm.Functor)
                        {
                            return false;
                        }

                        for (int index = 0; index < lhsCompoundTerm.Functor.Arity; ++index)
                        {
                            if (!Unify(lhsCompoundTerm.Children[index], rhsCompoundTerm.Children[index], testOnly))
                            {
                                return false;
                            }
                        }
                    }
                    else if (lhsValue != null && rhsValue != null)
                    {
                        if (!lhsValue.Object.Equals(rhsValue.Object))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
