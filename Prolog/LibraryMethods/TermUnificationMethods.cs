/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class TermUnificationMethods
    {
        public static bool Unify(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            return machine.Unify(arguments[0], arguments[1]);
        }

        public static bool CanUnify(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            return machine.CanUnify(arguments[0], arguments[1]);
        }

        public static bool CannotUnify(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            return machine.CannotUnify(arguments[0], arguments[1]);
        }

        public static bool Is(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            var lhs = arguments[0];
            var rhs = machine.Evaluate(arguments[1]);
            return machine.Unify(lhs, rhs);
        }

        public static bool Assert(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            var expression = machine.Evaluate(arguments[0]);
            var codeTerm = expression.GetCodeTerm();
            if (codeTerm != null)
            {
                var codeValue = codeTerm as CodeValue;
                if (codeValue != null)
                {
                    return Convert.ToBoolean(codeValue.Object);
                }
            }

            return false;
        }

        public static void Eval(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            var expression = arguments[0].Dereference();

            var compoundTerm = expression as WamCompoundTerm;
            if (compoundTerm == null)
            {
                return;
            }

            var builder = new WamInstructionStreamBuilder();
            builder.Write(new WamInstruction(WamInstructionOpCodes.Allocate));
            for (var idx = 0; idx < compoundTerm.Functor.Arity; ++idx)
            {
                builder.Write(new WamInstruction(
                    WamInstructionOpCodes.PutValue,
                    compoundTerm.Children[idx],
                    new WamInstructionRegister(WamInstructionRegisterTypes.Argument, (byte)idx)));
            }
            builder.Write(new WamInstruction(WamInstructionOpCodes.Call, compoundTerm.Functor));
            builder.Write(new WamInstruction(WamInstructionOpCodes.Deallocate));
            builder.Write(new WamInstruction(WamInstructionOpCodes.Proceed));

            machine.SetInstructionStream(builder.ToInstructionStream());
        }
    }
}
