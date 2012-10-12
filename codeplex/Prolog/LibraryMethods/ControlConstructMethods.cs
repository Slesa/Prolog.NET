/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class ControlConstructMethods
    {
        #region Public Methods

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

            WamReferenceTarget wamReferenceTargetFrom = arguments[1].Dereference();
            WamValueInteger wamValueIntegerFrom = wamReferenceTargetFrom as WamValueInteger;
            if (wamValueIntegerFrom == null)
            {
                yield break;
            }

            WamReferenceTarget wamReferenceTargetTo = arguments[2].Dereference();
            WamValueInteger wamValueIntegerTo = wamReferenceTargetTo as WamValueInteger;
            if (wamValueIntegerTo == null)
            {
                yield break;
            }

            for (int index = wamValueIntegerFrom.Value; index <= wamValueIntegerTo.Value; ++index)
            {
                WamValueInteger wamValueIntegerResult = WamValueInteger.Create(index);
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

        #endregion
    }
}
