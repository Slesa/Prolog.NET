/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class ValueComparisonMethods
    {
        public static CodeTerm Equal(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) == 0);
                    }
                }
                return new CodeValueBoolean(arguments[0] == arguments[1]);
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Unequal(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) != 0);
                    }
                }
                return new CodeValueBoolean(arguments[0] != arguments[1]);
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Less(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) < 0);
                    }
                }
                throw new ArgumentException("Arguments are not comparable.");
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm LessEqual(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) <= 0);
                    }
                }
                throw new ArgumentException("Arguments are not comparable.");
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Greater(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) > 0);
                    }
                }
                throw new ArgumentException("Arguments are not comparable.");
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm GreaterEqual(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);
            try
            {
                var argValue0 = arguments[0] as CodeValue;
                var argValue1 = arguments[1] as CodeValue;
                if (argValue0 != null && argValue1 != null)
                {
                    var lhs = argValue0.Object as IComparable;
                    var rhs = argValue1.Object as IComparable;
                    if (lhs != null && rhs != null)
                    {
                        return new CodeValueBoolean(lhs.CompareTo(rhs) >= 0);
                    }
                }
                throw new ArgumentException("Arguments are not comparable.");
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }
    }
}
