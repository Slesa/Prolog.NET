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
        #region Public Methods

        public static CodeTerm Substring(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length >= 2);
            Debug.Assert(arguments.Length <= 3);

            try
            {
                CodeValue argValue0 = (CodeValue)arguments[0];
                CodeValue argValue1 = (CodeValue)arguments[1];

                string value = Convert.ToString(argValue0.Object);
                int index = Convert.ToInt32(argValue1.Object);
                if (arguments.Length == 2)
                {
                    return new CodeValueString(value.Substring(index));
                }
                else // arguments.Length == 3
                {
                    CodeValue argValue2 = (CodeValue)arguments[2];

                    int length = Convert.ToInt32(argValue2.Object);
                    return new CodeValueString(value.Substring(index, length));
                }
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
                CodeValue argValue0 = (CodeValue)arguments[0];

                string value = Convert.ToString(argValue0.Object);
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
                CodeValue argValue0 = (CodeValue)arguments[0];
                CodeValue argValue1 = (CodeValue)arguments[1];

                string value = Convert.ToString(argValue0.Object);
                string substring = Convert.ToString(argValue1.Object);
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
                CodeValue argValue0 = (CodeValue)arguments[0];
                CodeValue argValue1 = (CodeValue)arguments[1];
                CodeValue argValue2 = (CodeValue)arguments[2];

                string value = Convert.ToString(argValue0.Object);
                string oldValue = Convert.ToString(argValue1.Object);
                string newValue = Convert.ToString(argValue2.Object);
                return new CodeValueString(value.Replace(oldValue, newValue));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        #endregion
    }
}
