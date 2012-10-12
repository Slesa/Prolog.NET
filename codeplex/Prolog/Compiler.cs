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
        #region Fields

        private WamInstructionStreamBuilder m_instructionStreamBuilder;
        private byte m_nextTemporaryRegisterId;
        private byte m_nextPermanentRegisterId;
        private Dictionary<string, WamInstructionRegister> m_variableAssignments;

        #endregion

        #region Public Methods

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
            bool isClause = (functor != null);

            if (isClause)
            {
                WamInstructionStreamClauseAttribute clauseAttribute = new WamInstructionStreamClauseAttribute(
                    m_instructionStreamBuilder.NextIndex,
                    functor,
                    index);
                m_instructionStreamBuilder.AddAttribute(clauseAttribute);
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
                for (int idx = 0; idx < codeSentence.Head.Children.Count; ++idx)
                {
                    Get(codeSentence.Head.Children[idx], GetArgumentRegister(idx));
                }
            }

            if (codeSentence.Body.Count > 0)
            {
                for (int idxProcedure = 0; idxProcedure < codeSentence.Body.Count; ++idxProcedure)
                {
                    CodeCompoundTerm codeCompoundTerm = codeSentence.Body[idxProcedure];

                    for (int idxArgument = 0; idxArgument < codeCompoundTerm.Children.Count; ++idxArgument)
                    {
                        Put(codeCompoundTerm.Children[idxArgument], GetArgumentRegister(idxArgument), libraries);
                    }

                    bool isLastCall = (idxProcedure == codeSentence.Body.Count - 1);

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

        #endregion

        #region Hidden Properties

        private WamInstructionStreamBuilder InstructionStreamBuilder
        {
            get { return m_instructionStreamBuilder; }
        }

        #endregion

        #region Hidden Methods

        private void Initialize()
        {

            m_instructionStreamBuilder = new WamInstructionStreamBuilder();
            m_nextTemporaryRegisterId = 0;
            m_nextPermanentRegisterId = 0;
            m_variableAssignments = new Dictionary<string, WamInstructionRegister>();
        }

        private void Put(CodeTerm codeTerm, WamInstructionRegister targetRegister, LibraryList libraries)
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

        private void Put(CodeValue codeValue, WamInstructionRegister targetRegister)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutValue, WamValue.Create(codeValue), targetRegister));
        }

        private void Put(CodeVariable codeVariable, WamInstructionRegister targetRegister)
        {
            WamInstructionRegister sourceRegister = GetRegisterAssignment(codeVariable.Name);
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

        private void Put(CodeCompoundTerm codeCompoundTerm, WamInstructionRegister targetRegister, LibraryList libraries)
        {
            WamInstructionRegister[] childrenRegisters = new WamInstructionRegister[codeCompoundTerm.Children.Count];

            // Build substructures.
            //
            for (int idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                CodeTerm child = codeCompoundTerm.Children[idx];

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

            Functor functor = Functor.Create(codeCompoundTerm.Functor);

            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.PutStructure, functor, targetRegister));
            for (int idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                CodeTerm child = codeCompoundTerm.Children[idx];

                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }

                if (child.IsCodeVariable)
                {
                    string variableName = child.AsCodeVariable.Name;
                    WamInstructionRegister variableRegister = GetRegisterAssignment(variableName);
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

        private void Get(CodeTerm codeTerm, WamInstructionRegister sourceRegister)
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

        private void Get(CodeValue codeValue, WamInstructionRegister sourceRegister)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetValue, sourceRegister, WamValue.Create(codeValue)));
        }

        private void Get(CodeVariable codeVariable, WamInstructionRegister sourceRegister)
        {
            WamInstructionRegister targetRegister = GetRegisterAssignment(codeVariable.Name);
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

        private void Get(CodeCompoundTerm codeCompoundTerm, WamInstructionRegister sourceRegister)
        {
            WamInstructionRegister[] childrenRegisters = new WamInstructionRegister[codeCompoundTerm.Children.Count];

            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.GetStructure, sourceRegister, Functor.Create(codeCompoundTerm.Functor)));
            for (int idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                CodeTerm child = codeCompoundTerm.Children[idx];

                if (child.IsCodeList)
                {
                    child = ConvertCodeList(child.AsCodeList);
                }

                if (child.IsCodeVariable)
                {
                    string variableName = child.AsCodeVariable.Name;
                    WamInstructionRegister variableRegister = GetRegisterAssignment(variableName);
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
            for (int idx = 0; idx < codeCompoundTerm.Children.Count; ++idx)
            {
                CodeTerm child = codeCompoundTerm.Children[idx];

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

        private void Call(CodeFunctor codeFunctor, LibraryList libraries)
        {
            Functor functor = Functor.Create(codeFunctor);

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

        private void Execute(CodeFunctor codeFunctor)
        {
            Functor functor = Functor.Create(codeFunctor);

            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Execute, functor));
        }

        private void Allocate()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Allocate));
        }

        private void Deallocate()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Deallocate));
        }

        private void Proceed()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Proceed));
        }

        private void Success()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Success));
        }

        private void Failure()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.Failure));
        }

        private void TryMeElse(Functor functor, int index)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.TryMeElse, functor, index));
        }

        private void RetryMeElse(Functor functor, int index)
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.RetryMeElse, functor, index));
        }

        private void TrustMe()
        {
            InstructionStreamBuilder.Write(new WamInstruction(WamInstructionOpCodes.TrustMe));
        }

        #endregion

        #region Hidden Members: Register Assignment

        private WamInstructionRegister GetArgumentRegister(int registerId)
        {
            return new WamInstructionRegister(WamInstructionRegisterTypes.Argument, (byte)registerId);
        }

        private WamInstructionRegister GetNextTemporaryRegister()
        {
            return new WamInstructionRegister(WamInstructionRegisterTypes.Temporary, m_nextTemporaryRegisterId++);
        }

        private WamInstructionRegister GetNextPermanentRegister(string name)
        {
            WamInstructionRegister instructionRegister = new WamInstructionRegister(WamInstructionRegisterTypes.Permanent, m_nextPermanentRegisterId++);

            m_variableAssignments.Add(name, instructionRegister);

            WamInstructionStreamVariableAttribute variableAttribute = new WamInstructionStreamVariableAttribute(
                InstructionStreamBuilder.NextIndex,
                name,
                instructionRegister);
            InstructionStreamBuilder.AddAttribute(variableAttribute);

            return instructionRegister;
        }

        private WamInstructionRegister GetRegisterAssignment(string name)
        {
            WamInstructionRegister instructionRegister;
            if (m_variableAssignments.TryGetValue(name, out instructionRegister))
            {
                return instructionRegister;
            }

            return WamInstructionRegister.Unused;
        }

        #endregion

        #region Hidden Members

        private CodeTerm ConvertCodeList(CodeList codeList)
        {
            CodeTerm result = codeList.Tail;

            for (int index = codeList.Head.Count - 1; index >= 0; --index)
            {
                result = new CodeCompoundTerm(
                    CodeFunctor.ListFunctor,
                    new CodeTerm[] { codeList.Head[index], result });
            }

            return result;
        }

        #endregion
    }
}
