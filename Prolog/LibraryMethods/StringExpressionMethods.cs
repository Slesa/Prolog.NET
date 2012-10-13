/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class StringExpressionMethods
    {
        public static CodeTerm Substring(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length >= 2);
            Debug.Assert(arguments.Length <= 3);

            try
            {
                var argValue0 = (CodeValue) arguments[0];
                var argValue1 = (CodeValue) arguments[1];

                var value = Convert.ToString(argValue0.Object);
                var index = Convert.ToInt32(argValue1.Object);
                if (arguments.Length == 2)
                {
                    return new CodeValueString(value.Substring(index));
                }
                // arguments.Length == 3

                var argValue2 = (CodeValue) arguments[2];
                var length = Convert.ToInt32(argValue2.Object);
                return new CodeValueString(value.Substring(index, length));

            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm StringLength(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];

                var value = Convert.ToString(argValue0.Object);
                return new CodeValueInteger(value.Length);
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm StringContains(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var value = Convert.ToString(argValue0.Object);

                var argValue1 = (CodeValue)arguments[1];
                var substring = Convert.ToString(argValue1.Object);

                return new CodeValueBoolean(value.Contains(substring));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm StringReplace(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 3);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var value = Convert.ToString(argValue0.Object);

                var argValue1 = (CodeValue)arguments[1];
                var oldValue = Convert.ToString(argValue1.Object);

                var argValue2 = (CodeValue)arguments[2];
                var newValue = Convert.ToString(argValue2.Object);

                return new CodeValueString(value.Replace(oldValue, newValue));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }
    }
}
