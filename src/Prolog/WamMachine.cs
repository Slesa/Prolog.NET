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
        const int DEFAULT_STACK_SIZE_LIMIT = 50000;
        const int MAX_STACK_SIZE_LIMIT = 1000000;

        int m_stackSizeLimit = DEFAULT_STACK_SIZE_LIMIT;

        readonly Stack<WamContext> _contextStack;
        readonly PerformanceStatistics _performanceStatistics;

        WamMachine(Program program, Query query)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            Program = program;
            Query = query;

            _contextStack = new Stack<WamContext>();
            CurrentContext = null;

            _performanceStatistics = new PerformanceStatistics();
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
            var wamMachine = new WamMachine(program, query);
            wamMachine.Initialize();
            return wamMachine;
        }

        public event EventHandler<WamMachineStepEventArgs> Stepped;

        public Program Program { get; private set; }
        public Query Query { get; private set; }

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
            get { return _performanceStatistics; }
        }

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

            var stepEventArgsState = new WamMachineStepEventArgsState();
            var stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            var results = ExecutionResults.None;

            var loop = true;
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

            var stepEventArgsState = new WamMachineStepEventArgsState();
            var stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            var results = ExecutionResults.None;

            var loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure || results == ExecutionResults.Success)
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
            var results = Step();
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

            var stepEventArgsState = new WamMachineStepEventArgsState();
            var stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            var results = ExecutionResults.None;

            var initialStackIndex = StackIndex;
            var loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure || results == ExecutionResults.Success)
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

            var stepEventArgsState = new WamMachineStepEventArgsState();
            var stepEventArgs = new WamMachineStepEventArgs(stepEventArgsState);

            var results = ExecutionResults.None;

            var initialStackIndex = StackIndex;
            var loop = true;
            while (loop)
            {
                results = Step();

                if (results == ExecutionResults.Failure || results == ExecutionResults.Success)
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

            var context = new WamContext();
            _contextStack.Push(context);
            CurrentContext = context;

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
            if (_contextStack.Count == 0)
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
            _contextStack.Pop();
            CurrentContext = _contextStack.Count == 0 ? null : _contextStack.Peek();
        }

        internal WamEnvironment GetEnvironment(int stackIndex)
        {
            if (stackIndex < 0 || stackIndex > StackIndex)
            {
                throw new ArgumentOutOfRangeException("stackIndex");
            }

            // Determine the offset relative to the current stack index.  0 indicates
            // the current stack index, 1 indicates the prior stack index, and so on.
            //
            var delta = StackIndex - stackIndex;

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
                // Adjust delta to account for the lack of a current stack frame.
                //
                delta -= 1;
            }

            // Search for the requested environment.
            //
            var environment = Environment;
            if (environment == null)
            {
                if (stackIndex == 0)
                {
                    return null;
                }
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
            var delta = StackIndex - stackIndex;

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
            var environment = Environment;
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

            var wamCompoundTerm = wamReferenceTarget.Dereference() as WamCompoundTerm;
            if (wamCompoundTerm == null)
            {
                return wamReferenceTarget;
            }

            if (!Program.Libraries.Contains(wamCompoundTerm.Functor))
            {
                return wamReferenceTarget;
            }

            var method = Program.Libraries[wamCompoundTerm.Functor];
            if (method == null
                || method.CanEvaluate == false)
            {
                return wamReferenceTarget;
            }

            var arguments = new WamReferenceTarget[method.Functor.Arity];
            for (var index = 0; index < method.Functor.Arity; ++index)
            {
                arguments[index] = Evaluate(wamCompoundTerm.Children[index]);
            }

            var function = method as Function;
            if (function != null)
            {
                var codeTerms = new CodeTerm[method.Functor.Arity];
                for (var index = 0; index < method.Functor.Arity; ++index)
                {
                    codeTerms[index] = arguments[index].GetCodeTerm();
                }

                CodeTerm result;
                try
                {
                    result = function.FunctionDelegate(codeTerms) ?? new CodeValueObject(null);
                }
                catch (Exception ex)
                {
                    result = new CodeValueException(ex);
                }

                return WamValue.Create(result);
            }
            
            var predicate = method as Predicate;
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

        int StackSizeLimit
        {
            get { return m_stackSizeLimit; }
            set
            {
                if (value < 0 || value > MAX_STACK_SIZE_LIMIT)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                m_stackSizeLimit = value;
            }
        }

        WamContext CurrentContext { get; set; }

        WamInstructionPointer InstructionPointer
        {
            get { return CurrentContext.InstructionPointer; }
            set { CurrentContext.InstructionPointer = value; }
        }

        IEnumerator<bool> PredicateEnumerator
        {
            get { return CurrentContext.PredicateEnumerator; }
            set { CurrentContext.PredicateEnumerator = value; }
        }

        WamInstructionPointer ReturnInstructionPointer
        {
            get { return CurrentContext.ReturnInstructionPointer; }
            set { CurrentContext.ReturnInstructionPointer = value; }
        }

        WamChoicePoint ChoicePoint
        {
            get { return CurrentContext.ChoicePoint; }
            set { CurrentContext.ChoicePoint = value; }
        }

        WamChoicePoint CutChoicePoint
        {
            get { return CurrentContext.CutChoicePoint; }
            set { CurrentContext.CutChoicePoint = value; }
        }

        WamCompoundTerm CurrentStructure
        {
            get { return CurrentContext.CurrentStructure; }
            set { CurrentContext.CurrentStructure = value; }
        }

        int CurrentStructureIndex
        {
            get { return CurrentContext.CurrentStructureIndex; }
            set { CurrentContext.CurrentStructureIndex = value; }
        }

        UnifyModes CurrentUnifyMode
        {
            get { return CurrentContext.CurrentUnifyMode; }
            set { CurrentContext.CurrentUnifyMode = value; }
        }

        int Generation
        {
            get { return ChoicePoint != null ? ChoicePoint.Generation : -1; }
        }

        void ClearContextStack()
        {
            _contextStack.Clear();
            CurrentContext = null;
        }

        WamVariable CreateVariable()
        {
            return new WamVariable(Generation);
        }

        WamCompoundTerm CreateCompoundTerm(Functor functor)
        {
            var value = WamCompoundTerm.Create(functor);
            CurrentStructure = value;
            CurrentStructureIndex = -1;
            return value;
        }

        WamInstructionPointer GetInstructionPointer(Functor functor, int index)
        {
            Procedure procedure;
            if (Program.Procedures.TryGetProcedure(functor, out procedure))
            {
                if (procedure.Clauses.Count >= index)
                {
                    return new WamInstructionPointer(procedure.Clauses[index].WamInstructionStream);
                }
            }
            return WamInstructionPointer.Undefined;
        }

        void SetRegister(WamInstructionRegister register, WamReferenceTarget value)
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

        WamReferenceTarget GetRegister(WamInstructionRegister register)
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

        WamReferenceTargetList GetPermanentVariables(int stackIndex)
        {
            var environment = GetEnvironment(stackIndex);
            return environment == null ? null : environment.PermanentRegisters;
        }

        ExecutionResults Execute(WamInstruction instruction)
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

        ExecutionResults Step()
        {
            switch (State)
            {
                case WamMachineStates.Backtrack:
                    if (Backtrack())
                    {
                        State = WamMachineStates.Run;
                        return ExecutionResults.None;
                    }
                    State = WamMachineStates.Halt;
                    return ExecutionResults.Failure;

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
                        var instruction = InstructionPointer.Instruction;
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

        void RaiseStepped(WamMachineStepEventArgs e)
        {
            if (Stepped != null)
            {
                Stepped(this, e);
            }
        }

        ExecutionResults OnAllocate(WamInstruction instruction)
        {
            Environment = new WamEnvironment(Environment, ReturnInstructionPointer, CutChoicePoint);
            ReturnInstructionPointer = WamInstructionPointer.Undefined;
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnCall(WamInstruction instruction)
        {
            var instructionPointer = GetInstructionPointer(instruction.Functor, 0);
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

        ExecutionResults OnCut(WamInstruction instruction)
        {
            while (ChoicePoint != CutChoicePoint)
            {
                ChoicePoint = ChoicePoint.Predecessor;
            }
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnExecute(WamInstruction instruction)
        {
            var instructionPointer = GetInstructionPointer(instruction.Functor, 0);
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

        ExecutionResults OnDeallocate(WamInstruction instruction)
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

        ExecutionResults OnLibraryCall(WamInstruction instruction)
        {
            var method = Program.Libraries[instruction.Functor];

            var function = method as Function;
            if (function != null)
            {
                return OnLibraryCallFunction(instruction, function);
            }

            var predicate = method as Predicate;
            if (predicate != null)
            {
                return OnLibraryCallPredicate(instruction, predicate);
            }

            var backtrackingPredicate = method as BacktrackingPredicate;
            if (backtrackingPredicate != null)
            {
                return OnLibraryCallBacktrackingPredicate(instruction, backtrackingPredicate);
            }

            var codePredicate = method as CodePredicate;
            if (codePredicate != null)
            {
                return OnLibraryCallCodePredicate(instruction, codePredicate);
            }

            // Library method not found.
            //
            return ExecutionResults.Failure;
        }

        ExecutionResults OnLibraryCallPredicate(WamInstruction instruction, Predicate predicate)
        {
            var arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (var index = 0; index < instruction.Functor.Arity; ++index)
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

        ExecutionResults OnLibraryCallCodePredicate(WamInstruction instruction, CodePredicate predicate)
        {
            var arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (var index = 0; index < instruction.Functor.Arity; ++index)
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

        ExecutionResults OnLibraryCallBacktrackingPredicate(WamInstruction instruction, BacktrackingPredicate predicate)
        {
            var arguments = new WamReferenceTarget[instruction.Functor.Arity];
            for (var index = 0; index < instruction.Functor.Arity; ++index)
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

            var enumerator = enumerable.GetEnumerator();

            ChoicePoint = new WamChoicePoint(ChoicePoint, Environment, StackIndex, ReturnInstructionPointer, ArgumentRegisters, CutChoicePoint)
                              {
                                  BacktrackInstructionPointer = InstructionPointer.GetNext(),
                                  PredicateEnumerator = enumerator
                              };
            InstructionPointer = ChoicePoint.BacktrackInstructionPointer;
            PredicateEnumerator = ChoicePoint.PredicateEnumerator;
            return ExecutionResults.None;
        }

        ExecutionResults OnLibraryCallFunction(WamInstruction instruction, Function function)
        {
            var functionArguments = new CodeTerm[instruction.Functor.Arity];
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
                var functionResultValue = (CodeValue)functionResult;
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

        ExecutionResults OnNoop(WamInstruction instruction)
        {
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnProceed(WamInstruction instruction)
        {
            StackIndex -= 1;
            InstructionPointer = ReturnInstructionPointer;

            ReturnInstructionPointer = WamInstructionPointer.Undefined;
            TemporaryRegisters.Clear();
            return ExecutionResults.None;
        }

        ExecutionResults OnRetryMeElse(WamInstruction instruction)
        {
            var instructionPointer = GetInstructionPointer(instruction.Functor, instruction.Index);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }
            ChoicePoint.BacktrackInstructionPointer = instructionPointer;
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnSuccess(WamInstruction instruction)
        {
            return ExecutionResults.Success;
        }

        ExecutionResults OnFailure(WamInstruction instruction)
        {
            return ExecutionResults.Failure;
        }

        ExecutionResults OnTrustMe(WamInstruction instruction)
        {
            if (ChoicePoint == null)
            {
                throw new InvalidOperationException("Choice point not found.");
            }
            ChoicePoint = ChoicePoint.Predecessor;
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnTryMeElse(WamInstruction instruction)
        {
            var instructionPointer = GetInstructionPointer(instruction.Functor, instruction.Index);
            if (instructionPointer == WamInstructionPointer.Undefined)
            {
                return ExecutionResults.Failure;
            }

            ChoicePoint = new WamChoicePoint(ChoicePoint, Environment, StackIndex, ReturnInstructionPointer, ArgumentRegisters, CutChoicePoint)
                              {
                                  BacktrackInstructionPointer = instructionPointer
                              };
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnPutValue(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, instruction.ReferenceTarget);
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnPutStructure(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, CreateCompoundTerm(instruction.Functor));
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnPutBoundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnPutUnboundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.SourceRegister, CreateVariable());
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnSetBoundVariable(WamInstruction instruction)
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

        ExecutionResults OnSetUnboundVariable(WamInstruction instruction)
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

        ExecutionResults OnSetValue(WamInstruction instruction)
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

        ExecutionResults OnGetValue(WamInstruction instruction)
        {
            if (!(Unify(instruction.ReferenceTarget, GetRegister(instruction.SourceRegister))))
            {
                return ExecutionResults.Backtrack;
            }
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnGetStructure(WamInstruction instruction)
        {
            var sourceReference = GetRegister(instruction.SourceRegister).Dereference();
            var sourceVariable = sourceReference as WamVariable;
            var sourceCompoundTerm = sourceReference as WamCompoundTerm;

            // Ensure target is either a variable or compound term.
            //
            Debug.Assert(sourceVariable != null || sourceCompoundTerm != null);

            if (sourceVariable != null)
            {
                var compoundTerm = WamCompoundTerm.Create(instruction.Functor);
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

        ExecutionResults OnGetBoundVariable(WamInstruction instruction)
        {
            if (!(Unify(GetRegister(instruction.TargetRegister), GetRegister(instruction.SourceRegister))))
            {
                return ExecutionResults.Backtrack;
            }
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnGetUnboundVariable(WamInstruction instruction)
        {
            SetRegister(instruction.TargetRegister, GetRegister(instruction.SourceRegister));
            InstructionPointer = InstructionPointer.GetNext();
            return ExecutionResults.None;
        }

        ExecutionResults OnUnifyBoundVariable(WamInstruction instruction)
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

        ExecutionResults OnUnifyValue(WamInstruction instruction)
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

        ExecutionResults OnUnifyUnboundVariable(WamInstruction instruction)
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

        void Bind(WamVariable variable, WamReferenceTarget target)
        {
            variable.Bind(target);
            Trail(variable);
        }

        void Trail(WamVariable variable)
        {
            if (ChoicePoint == null) return;

            if (variable.Generation <= ChoicePoint.Generation)
            {
                ChoicePoint.Trail.Add(variable);
            }
        }

        void UnwindTrail()
        {
            if (ChoicePoint == null) return;

            foreach (WamVariable variable in ChoicePoint.Trail)
            {
                variable.Unbind();
            }
            ChoicePoint.Trail.Clear();
        }

        bool Backtrack()
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

        bool Unify(WamReferenceTarget lhs, WamReferenceTarget rhs, bool testOnly)
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

            var lhsVariable = lhs as WamVariable;
            var rhsVariable = rhs as WamVariable;

            var lhsCompoundTerm = lhs as WamCompoundTerm;
            var rhsCompoundTerm = rhs as WamCompoundTerm;

            var lhsValue = lhs as WamValue;
            var rhsValue = rhs as WamValue;

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
    }
}
