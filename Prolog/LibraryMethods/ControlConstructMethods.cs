/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;
using System.Diagnostics;

namespace Prolog
{
    internal static class ControlConstructMethods
    {
        public static bool True(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 0);
            return true;
        }

        public static bool Fail(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 0);
            return false;
        }

        public static IEnumerable<bool> For(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            var wamReferenceTargetFrom = arguments[1].Dereference();
            var wamValueIntegerFrom = wamReferenceTargetFrom as WamValueInteger;
            if (wamValueIntegerFrom == null)
            {
                yield break;
            }

            var wamReferenceTargetTo = arguments[2].Dereference();
            var wamValueIntegerTo = wamReferenceTargetTo as WamValueInteger;
            if (wamValueIntegerTo == null)
            {
                yield break;
            }

            for (var index = wamValueIntegerFrom.Value; index <= wamValueIntegerTo.Value; ++index)
            {
                var wamValueIntegerResult = WamValueInteger.Create(index);
                if (machine.Unify(arguments[0], wamValueIntegerResult))
                {
                    yield return true;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
