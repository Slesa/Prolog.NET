/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class TypeConversionExpressionMethods
    {
        public static CodeTerm GetType(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var typeName = Convert.ToString(argValue0.Object);
                return new CodeValueType(Type.GetType(typeName));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm TypeOf(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue) arguments[0];
                var value = argValue0.Object;
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                return new CodeValueType(value.GetType());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ToInteger(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                return new CodeValueInteger(Convert.ToInt32(argValue0.Object));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ToDouble(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                return new CodeValueDouble(Convert.ToDouble(argValue0.Object));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ToBoolean(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                return new CodeValueBoolean(Convert.ToBoolean(argValue0.Object));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ToString(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length >= 1);
            Debug.Assert(arguments.Length <= 2);

            try
            {
                var argValue0 = (CodeValue) arguments[0];
                var value = argValue0.Object;
                if (arguments.Length == 1)
                {
                    return new CodeValueString(Convert.ToString(value));
                }
                // arguments.Length == 2

                var argValue1 = (CodeValue) arguments[1];
                var format = Convert.ToString(argValue1.Object);
                return new CodeValueString(string.Format(format, value));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ToDate(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1 || arguments.Length == 3);

            try
            {
                if (arguments.Length == 1)
                {
                    var argValue0 = (CodeValue)arguments[0];
                    var value = argValue0.Object;
                    return new CodeValueDateTime(Convert.ToDateTime(value));
                }
                else // arguments.Length == 3
                {
                    var argValue0 = (CodeValue)arguments[0];
                    var year = Convert.ToInt32(argValue0.Object);
                    var argValue1 = (CodeValue)arguments[1];
                    var month= Convert.ToInt32(argValue1.Object);
                    var argValue2 = (CodeValue)arguments[2];
                    var day = Convert.ToInt32(argValue2.Object);
                    return new CodeValueDateTime(new DateTime(year, month, day));
                }
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Ceiling(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var argInteger0 = argValue0 as CodeValueInteger;
                if (argInteger0 != null)
                {
                    return argInteger0;
                }

                var argDouble0 = argValue0 as CodeValueDouble;
                return argDouble0 != null 
                    ? new CodeValueInteger(Convert.ToInt32(Math.Ceiling(argDouble0.Value))) 
                    : new CodeValueInteger(Convert.ToInt32(Math.Ceiling(Convert.ToDouble(argValue0.Object))));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Floor(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var argInteger0 = argValue0 as CodeValueInteger;
                if (argInteger0 != null)
                {
                    return argInteger0;
                }

                var argDouble0 = argValue0 as CodeValueDouble;
                return argDouble0 != null 
                    ? new CodeValueInteger(Convert.ToInt32(Math.Floor(argDouble0.Value))) 
                    : new CodeValueInteger(Convert.ToInt32(Math.Floor(Convert.ToDouble(argValue0.Object))));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Round(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var argInteger0 = argValue0 as CodeValueInteger;
                if (argInteger0 != null)
                {
                    return argInteger0;
                }

                var argDouble0 = argValue0 as CodeValueDouble;
                return argDouble0 != null 
                    ? new CodeValueInteger(Convert.ToInt32(Math.Round(argDouble0.Value))) 
                    : new CodeValueInteger(Convert.ToInt32(Math.Round(Convert.ToDouble(argValue0.Object))));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Truncate(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                var argValue0 = (CodeValue)arguments[0];
                var argInteger0 = argValue0 as CodeValueInteger;
                if (argInteger0 != null)
                {
                    return argInteger0;
                }

                var argDouble0 = argValue0 as CodeValueDouble;
                return argDouble0 != null 
                    ? new CodeValueInteger(Convert.ToInt32(Math.Truncate(argDouble0.Value))) 
                    : new CodeValueInteger(Convert.ToInt32(Math.Truncate(Convert.ToDouble(argValue0.Object))));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }
    }
}
