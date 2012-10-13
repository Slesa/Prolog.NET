/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Prolog
{
    internal static class AllSolutionMethods
    {
        public static bool FindAll(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            var arg0 = arguments[0].Dereference();
            var arg1 = arguments[1].Dereference();
            var arg2 = arguments[2].Dereference();

            var variable = arg0 as WamVariable;
            if (variable == null)
            {
                return false;
            }

            var goal = arg1 as WamCompoundTerm;
            if (goal == null)
            {
                return false;
            }

            var result = arg2 as WamVariable;
            if (result == null)
            {
                return false;
            }

            var builder = new WamInstructionStreamBuilder();
            builder.Write(new WamInstruction(WamInstructionOpCodes.Allocate));
            for (var idx = 0; idx < goal.Functor.Arity; ++idx)
            {
                builder.Write(new WamInstruction(
                    WamInstructionOpCodes.PutValue,
                    goal.Children[idx],
                    new WamInstructionRegister(WamInstructionRegisterTypes.Argument, (byte)idx)));
            }
            builder.Write(new WamInstruction(WamInstructionOpCodes.Call, goal.Functor));
            builder.Write(new WamInstruction(WamInstructionOpCodes.Success));

            machine.PushContext(builder.ToInstructionStream());

            var values = new List<WamReferenceTarget>();

            try
            {
                var results = machine.RunToSuccess();
                while (results == ExecutionResults.Success)
                {
                    var value = variable.Clone();
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
    }
}
