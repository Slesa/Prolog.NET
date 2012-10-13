/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

namespace Prolog
{
    internal static class RandomNumberMethods
    {
        static Random s_random = new Random();
        static int s_seed = -1;

        public static bool Randomize(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 0);

            s_seed = -1;
            s_random = new Random();

            return true;
        }

        public static bool GetSeed(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            var operand = arguments[0];
            var seed = WamValueInteger.Create(s_seed);

            return machine.Unify(operand, seed);
        }

        public static bool SetSeed(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            var operand = arguments[0].Dereference() as WamValueInteger;
            if (operand == null)
            {
                return false;
            }

            s_seed = operand.Value;
            s_random = new Random(s_seed);

            return true;
        }

        public static bool NextDouble(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            var operand = arguments[0];
            var value = WamValueDouble.Create(s_random.NextDouble());

            return machine.Unify(operand, value);
        }

        public static bool Next(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            var minValue = arguments[0].Dereference() as WamValueInteger;
            if (minValue == null)
            {
                return false;
            }

            var maxValue = arguments[1].Dereference() as WamValueInteger;
            if (maxValue == null)
            {
                return false;
            }

            var operand = arguments[2];
            var value = WamValueInteger.Create(s_random.Next(minValue.Value, maxValue.Value));

            return machine.Unify(operand, value);
        }
    }
}
