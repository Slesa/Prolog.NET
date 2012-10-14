/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;

using Prolog.Code;

namespace Prolog
{
    internal sealed class Compiler
    {
        byte _nextTemporaryRegisterId;
        byte _nextPermanentRegisterId;
        Dictionary<string, WamInstructionRegister> _variableAssignments;

        public WamInstructionStream Compile(CodeSentence codeSentence, LibraryList libraries, bool optimize)
        {
            return Compile(codeSentence, null, -1, false, libraries, optimize);
        }

        public WamInstructionStream Compile(CodeSentence codeSentence, Functor functor, int index, bool isLast, LibraryList libraries, bool optimize)
        {
            Initialize();

            // When true, indicates we are compiling code for a procedure clause.  When false, indicates we
            // are compiling for an ad hoc query.
            //
            var isClause = (functor != null);

            if (isClause)
            {
                var clauseAttribute = new WamInstructionStreamClauseAttribute(
                    InstructionStreamBuilder.NextIndex,
                    functor,
                    index);
                InstructionStreamBuilder.AddAttribute(clauseAttribute);
            }

            if (isClause)
            {
                if (isLast)
                {
                    if (index == 0)
                    {
                        // Procedure only has one clause in it.  No retry logic required.
                    }
                    else
                    {
                        TrustMe();
                    }
                }
                else
                {
                    if (index == 0)
                    {
                        TryMeElse(functor, index + 1);
                    }
                    else
                    {
                        RetryMeElse(functor, index + 1);
                    }
                }
            }

            Allocate();

            if (codeSentence.Head != null)
            {
                for (var idx = 0; idx < codeSentence.Head.Children.Count; ++idx)
                {
                    Get(codeSentence.Head.Children[idx], GetArgumentRegister(idx));
                }
            }

            if (codeSentence.Body.Count > 0)
            {
                for (var idxProcedure = 0; idxProcedure < codeSentence.Body.Count; ++idxProcedure)
                {
                    var codeCompoundTerm = codeSentence.Body[idxProcedure];

                    for (var idxArgument = 0; idxArgument < codeCompoundTerm.Children.Count; ++idxArgument)
                    {
                        Put(codeCompoundTerm.Children[idxArgument], GetArgumentRegister(idxArgument), libraries);
                    }

                    var isLastCall = (idxProcedure == codeSentence.Body.Count - 1);
                    if (isClause)
                    {
                        if (isLastCall)
                        {
                            if (optimize
                                && !libraries.Contains(Functor.Create(codeCompoundTerm.Functor))
                                && codeCompoundTerm.Functor != CodeFunctor.CutFunctor)
                            {
                                Deallocate();
                                Execute(codeCompoundTerm.Functor);
                            }
                            else
                            {
                                Call(codeCompoundTerm.Functor, libraries);
                                Deallocate();
                                Proceed();
                            }
                        }
                        else
                        {
                            Call(codeCompoundTerm.Functor, libraries);
                        }
                    }
                    else // isQuery
                    {
                        Call(codeCompoundTerm.Functor, libraries);

                        if (isLastCall)
                        {
                            Success();
                        }
                    }
                }
            }
            else // fact
            {
                if (isClause)
                {
                    Deallocate();
                    Proceed();
                }
                else // isQuery
                {
                    // No action required.
                }
            }

            return InstructionStreamBuilder.ToInstructionStream();
        }

        private WamInstructionStreamBuilder InstructionStreamBuilder { get; set; }

        void Initialize()
        {

            InstructionStreamBuilder = new WamInstructionStreamBuilder();
            _nextTemporaryRegisterId = 0;
            _nextPermanentRegisterId = 0;
            _variableAssignments = new Dictionary<string, WamInstructionRegister>();
        }

        void Put(CodeTerm codeTerm, WamInstructionRegister targetRegister, LibraryList libraries)
        {
            if (codeTerm.IsCodeList)
            {
                codeTerm = ConvertCodeList(codeTerm.AsCodeList);
            }

            if (codeTerm.IsCodeVariable)
            {
                Put(codeTerm.AsCodeVariable, targetRegister);
                return;
            }

            if (codeTerm.IsCodeCompoundTerm)
            {
                Put(codeTerm.AsCodeCompoundTerm, targetRegister, libraries);
                return;
            }

            if (codeTerm.IsCodeValue)
            {
                Put(codeTerm.AsCodeValue, targetRegister);
                return;
            }

            throw new InvalidOperationException("Unsupported codeTerm type.");
        }

        void Put(CodeValue codeValue, WamInstructionRegister targetRegister)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutValue, WamValue.Create(codeValue), targetRegister));
        }

        void Put(CodeVariable codeVariable, WamInstructionRegister targetRegister)
        {
            var sourceRegister = GetRegisterAssignment(codeVariable.Name);
            if (sourceRegister.IsUnused)
            {
                sourceRegister = GetNextPermanentRegister(codeVariable.Name);
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutUnboundVariable, sourceRegister, targetRegister));
            }
            else
            {
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutBoundVariable, sourceRegister, targetRegister));
            }
        }

        void Put(CodeCompoundTerm codeCompoundTerm, WamInstructionRegister targetRegister, LibraryList libraries)
        {
            var childrenRegisters = new WamInstructionRegister[codeCompoundTerm.Children.Count];

            // Build substructures.
            //
            for (var idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                var child = codeCompoundTerm.Children[idx];
                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }
                if (child.IsCodeCompoundTerm)
                {
                    childrenRegisters[idx] = GetNextTemporaryRegister();
                    Put(child, childrenRegisters[idx], libraries);
                }
            }

            var functor = Functor.Create(codeCompoundTerm.Functor);

            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutStructure, functor, targetRegister));
            for (var idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                var child = codeCompoundTerm.Children[idx];

                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }
                if (child.IsCodeVariable)
                {
                    var variableName = child.AsCodeVariable.Name;
                    var variableRegister = GetRegisterAssignment(variableName);
                    if (variableRegister.IsUnused)
                    {
                        variableRegister = GetNextPermanentRegister(variableName);
                        InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.SetUnboundVariable, variableRegister));
                    }
                    else
                    {
                        InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.SetBoundVariable, variableRegister));
                    }
                }
                else if (child.IsCodeCompoundTerm)
                {
                    InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.SetBoundVariable, childrenRegisters[idx]));
                }
                else if (child.IsCodeValue)
                {
                    InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.SetValue, WamValue.Create(child.AsCodeValue)));
                }
                else
                {
                    throw new InvalidOperationException("Unsupported codeTerm type.");
                }
            }
        }

        void Get(CodeTerm codeTerm, WamInstructionRegister sourceRegister)
        {
            if (codeTerm.IsCodeList)
            {
                codeTerm = ConvertCodeList(codeTerm.AsCodeList);
            }
            if (codeTerm.IsCodeVariable)
            {
                Get(codeTerm.AsCodeVariable, sourceRegister);
                return;
            }
            if (codeTerm.IsCodeCompoundTerm)
            {
                Get(codeTerm.AsCodeCompoundTerm, sourceRegister);
                return;
            }
            if (codeTerm.IsCodeValue)
            {
                Get(codeTerm.AsCodeValue, sourceRegister);
                return;
            }
            throw new InvalidOperationException("Unsupported codeTerm type.");
        }

        void Get(CodeValue codeValue, WamInstructionRegister sourceRegister)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetValue, sourceRegister, WamValue.Create(codeValue)));
        }

        void Get(CodeVariable codeVariable, WamInstructionRegister sourceRegister)
        {
            var targetRegister = GetRegisterAssignment(codeVariable.Name);
            if (targetRegister.IsUnused)
            {
                targetRegister = GetNextPermanentRegister(codeVariable.Name);
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetUnboundVariable, sourceRegister, targetRegister));
            }
            else
            {
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetBoundVariable, sourceRegister, targetRegister));
            }
        }

        void Get(CodeCompoundTerm codeCompoundTerm, WamInstructionRegister sourceRegister)
        {
            var childrenRegisters = new WamInstructionRegister[codeCompoundTerm.Children.Count];

            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetStructure, sourceRegister, Functor.Create(codeCompoundTerm.Functor)));
            for (var idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                var child = codeCompoundTerm.Children[idx];
                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }
                if (child.IsCodeVariable)
                {
                    var variableName = child.AsCodeVariable.Name;
                    var variableRegister = GetRegisterAssignment(variableName);
                    if (variableRegister.IsUnused)
                    {
                        variableRegister = GetNextPermanentRegister(variableName);
                        InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.UnifyUnboundVariable, variableRegister));
                    }
                    else
                    {
                        InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.UnifyBoundVariable, variableRegister));
                    }
                }
                else if (child.IsCodeCompoundTerm)
                {
                    childrenRegisters[idx] = GetNextTemporaryRegister();
                    InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.UnifyUnboundVariable, childrenRegisters[idx]));
                }
                else if (child.IsCodeValue)
                {
                    InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.UnifyValue, WamValue.Create(child.AsCodeValue)));
                }
                else
                {
                    throw new InvalidOperationException("Unsupported codeTerm type.");
                }
            }

            // Build substructures.
            //
            for (var idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                var child = codeCompoundTerm.Children[idx];
                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }
                if (child.IsCodeCompoundTerm)
                {
                    Get(child, childrenRegisters[idx]);
                }
            }
        }

        void Call(CodeFunctor codeFunctor, LibraryList libraries)
        {
            var functor = Functor.Create(codeFunctor);
            if (functor == Functor.CutFunctor)
            {
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Cut));
            }
            else if (libraries.Contains(functor))
            {
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.LibraryCall, functor));
            }
            else
            {
                InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Call, functor));
            }
        }

        void Execute(CodeFunctor codeFunctor)
        {
            var functor = Functor.Create(codeFunctor);
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Execute, functor));
        }

        void Allocate()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Allocate));
        }

        void Deallocate()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Deallocate));
        }

        void Proceed()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Proceed));
        }

        void Success()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Success));
        }

        void Failure()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Failure));
        }

        void TryMeElse(Functor functor, int index)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.TryMeElse, functor, index));
        }

        void RetryMeElse(Functor functor, int index)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.RetryMeElse, functor, index));
        }

        void TrustMe()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.TrustMe));
        }

        WamInstructionRegister GetArgumentRegister(int registerId)
        {
            return new WamInstructionRegister(WamInstructionRegisterTypes.Argument, (byte)registerId);
        }

        WamInstructionRegister GetNextTemporaryRegister()
        {
            return new WamInstructionRegister(WamInstructionRegisterTypes.Temporary, _nextTemporaryRegisterId++);
        }

        WamInstructionRegister GetNextPermanentRegister(string name)
        {
            var instructionRegister = new WamInstructionRegister(WamInstructionRegisterTypes.Permanent, _nextPermanentRegisterId++);
            _variableAssignments.Add(name, instructionRegister);

            var variableAttribute = new WamInstructionStreamVariableAttribute(
                InstructionStreamBuilder.NextIndex,
                name,
                instructionRegister);
            InstructionStreamBuilder.AddAttribute(variableAttribute);
            return instructionRegister;
        }

        WamInstructionRegister GetRegisterAssignment(string name)
        {
            WamInstructionRegister instructionRegister;
            return _variableAssignments.TryGetValue(name, out instructionRegister) ? instructionRegister : WamInstructionRegister.Unused;
        }

        CodeTerm ConvertCodeList(CodeList codeList)
        {
            var result = codeList.Tail;
            for (var index = codeList.Head.Count - 1; index >= 0; --index)
            {
                result = new CodeCompoundTerm(
                    CodeFunctor.ListFunctor,
                    new[] { codeList.Head[index], result });
            }
            return result;
        }
    }
}
