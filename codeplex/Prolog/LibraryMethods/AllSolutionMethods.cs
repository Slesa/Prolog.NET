/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class AllSolutionMethods
    {
        #region Public Methods

        public static bool FindAll(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            WamReferenceTarget arg0 = arguments[0].Dereference();
            WamReferenceTarget arg1 = arguments[1].Dereference();
            WamReferenceTarget arg2 = arguments[2].Dereference();

            WamVariable variable = arg0 as WamVariable;
            if (variable == null)
            {
                return false;
            }

            WamCompoundTerm goal = arg1 as WamCompoundTerm;
            if (goal == null)
            {
                return false;
            }

            WamVariable result = arg2 as WamVariable;
            if (result == null)
            {
                return false;
            }

            WamInstructionStreamBuilder builder = new WamInstructionStreamBuilder();
            builder.Write(new WamInstruction(WamInstructionOpCodes.Allocate));
            for (int idx = 0; idx < goal.Functor.Arity; ++idx)
            {
                builder.Write(new WamInstruction(
                    WamInstructionOpCodes.PutValue,
                    goal.Children[idx],
                    new WamInstructionRegister(WamInstructionRegisterTypes.Argument, (byte)idx)));
            }
            builder.Write(new WamInstruction(WamInstructionOpCodes.Call, goal.Functor));
            builder.Write(new WamInstruction(WamInstructionOpCodes.Success));

            machine.PushContext(builder.ToInstructionStream());

            List<WamReferenceTarget> values = new List<WamReferenceTarget>();

            try
            {
                ExecutionResults results = machine.RunToSuccess();
                while (results == ExecutionResults.Success)
                {
                    WamReferenceTarget value = variable.Clone();
                    values.Add(value);

                    results = machine.RunToSuccess();
                }
            }
            finally
            {
                machine.PopContext(true);
            }

            // Unbind the variable from the last results found by the goal.
            //
            variable.Unbind();

            // Unify the output variable with the list of values.
            //
            return machine.Unify(result, WamReferenceTarget.Create(values));
        }

        #endregion
    }
}
