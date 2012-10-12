/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Diagnostics;

using Prolog.Code;

namespace Prolog
{
    internal static class TypeValueTestingMethods
    {
        #region Public Methods

        public static bool Var(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return wamReferenceTarget is WamVariable;
        }

        public static bool Nonvar(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return !(wamReferenceTarget is WamVariable);
        }

        public static bool Atom(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                return wamCompoundTerm.Functor.Arity == 0;
            }

            return false;
        }

        public static bool Integer(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return wamReferenceTarget is WamValueInteger;
        }

        public static bool Float(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return wamReferenceTarget is WamValueDouble;
        }

        public static bool Number(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return wamReferenceTarget is WamValueInteger
                   || wamReferenceTarget is WamValueDouble;
        }

        public static bool Atomic(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                return wamCompoundTerm.Functor.Arity == 0;
            }

            return wamReferenceTarget is WamValueInteger
                   || wamReferenceTarget is WamValueDouble;
        }

        public static bool Compound(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                return wamCompoundTerm.Functor.Arity > 0;
            }

            return false;
        }

        public static bool Callable(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                return true;
            }

            return false;
        }

        public static bool List(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return IsList(wamReferenceTarget);
        }

        public static bool PartialList(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return IsPartialList(wamReferenceTarget);
        }

        public static bool ListOrPartialList(WamMachine machine, WamReferenceTarget[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            WamReferenceTarget wamReferenceTarget = arguments[0].Dereference();

            return IsListOrPartialList(wamReferenceTarget);
        }

        public static CodeTerm IsType(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 2);

            try
            {
                CodeValue argValue0 = (CodeValue)arguments[0];
                CodeValueType argValue1 = (CodeValueType)arguments[1];
                object value = argValue0.Object;
                Type type = argValue1.Value;
                return new CodeValueBoolean(type.IsInstanceOfType(value));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm IsNull(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                CodeValue argValue0 = (CodeValue)arguments[0];
                object value = argValue0.Object;
                return new CodeValueBoolean(value == null);
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm IsEmpty(CodeTerm[] arguments)
        {
            Debug.Assert(arguments.Length == 1);

            try
            {
                CodeValue argValue0 = (CodeValue)arguments[0];

                string value = Convert.ToString(argValue0.Object);
                return new CodeValueBoolean(string.IsNullOrEmpty(value));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        #endregion

        #region Hidden Members

        private static bool IsList(WamReferenceTarget wamReferenceTarget)
        {
            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                if (wamCompoundTerm.Functor == Functor.NilFunctor)
                {
                    return true;
                }

                if (wamCompoundTerm.Functor == Functor.ListFunctor)
                {
                    WamReferenceTarget tail = wamCompoundTerm.Children[1].Dereference();

                    return IsList(tail);
                }
            }

            return false;
        }

        private static bool IsPartialList(WamReferenceTarget wamReferenceTarget)
        {
            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                if (wamCompoundTerm.Functor == Functor.ListFunctor)
                {
                    WamReferenceTarget tail = wamCompoundTerm.Children[1].Dereference();

                    return tail is WamVariable
                           || IsPartialList(tail);
                }
            }

            return false;
        }

        private static bool IsListOrPartialList(WamReferenceTarget wamReferenceTarget)
        {
            WamCompoundTerm wamCompoundTerm = wamReferenceTarget as WamCompoundTerm;
            if (wamCompoundTerm != null)
            {
                if (wamCompoundTerm.Functor == Functor.NilFunctor)
                {
                    return true;
                }

                if (wamCompoundTerm.Functor == Functor.ListFunctor)
                {
                    WamReferenceTarget tail = wamCompoundTerm.Children[1].Dereference();

                    return tail is WamVariable
                           || IsListOrPartialList(tail);
                }
            }

            return false;
        }

        #endregion
    }
}
