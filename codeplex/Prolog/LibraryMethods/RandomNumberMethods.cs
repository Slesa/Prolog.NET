/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class RandomNumberMethods
    {
        #region Fields

        private static Random s_random = new Random();
        private static int s_seed = -1;

        #endregion

        #region Public Methods

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

            WamReferenceTarget operand = arguments[0];

            WamValueInteger seed = WamValueInteger.Create(s_seed);

            return machine.Unify(operand, seed);
        }

        public static bool SetSeed(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamValueInteger operand = arguments[0].Dereference() as WamValueInteger;
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

            WamReferenceTarget operand = arguments[0];

            WamValueDouble value = WamValueDouble.Create(s_random.NextDouble());

            return machine.Unify(operand, value);
        }

        public static bool Next(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            WamValueInteger minValue = arguments[0].Dereference() as WamValueInteger;
            if (minValue == null)
            {
                return false;
            }

            WamValueInteger maxValue = arguments[1].Dereference() as WamValueInteger;
            if (maxValue == null)
            {
                return false;
            }

            WamReferenceTarget operand = arguments[2];

            WamValueInteger value = WamValueInteger.Create(s_random.Next(minValue.Value, maxValue.Value));

            return machine.Unify(operand, value);
        }

        #endregion
    }
}
