/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Prolog.Code;

namespace Prolog
{
    internal static class ArithmeticExpressionMethods
    {
        #region Public Methods

        public static CodeTerm Negate(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Negate());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Increment(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Increment());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Decrement(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Decrement());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Add(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Add(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Subtract(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Subtract(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Multiply(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Multiply(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Divide(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Divide(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm IntegerDivide(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.IntegerDivide(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Remainder(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Remainder(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Modulo(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Modulo(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm BitwiseAnd(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.BitwiseAnd(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm BitwiseOr(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.BitwiseOr(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm BitwiseXor(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.BitwiseXor(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm BitwiseNot(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.BitwiseNot());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ShiftLeft(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.ShiftLeft(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm IntegerShiftRight(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.IntegerShiftRight(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm BitwiseShiftRight(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.BitwiseShiftRight(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm AbsoluteValue(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.AbsoluteValue());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Sign(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Sign());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Minimum(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Minimum(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Maximum(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Maximum(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Power(CodeTerm[] arguments)
        {
            try
            {
                Operand lhs;
                Operand rhs;
                GetOperands(arguments, out lhs, out rhs);

                return CodeValue.Create(lhs.Power(rhs));
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm SquareRoot(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.SquareRoot());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ArcTangent(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.ArcTangent());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Cosign(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Cosine());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ArcCosign(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.ArcCosine());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Sine(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Sine());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm ArcSine(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.ArcSine());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Exp(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Exp());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        public static CodeTerm Log(CodeTerm[] arguments)
        {
            try
            {
                Operand operand;
                GetOperand(arguments, out operand);

                return CodeValue.Create(operand.Log());
            }
            catch (Exception ex)
            {
                return new CodeValueException(ex);
            }
        }

        #endregion

        #region Hidden Members

        private static void GetOperand(CodeTerm[] arguments, out Operand operand)
        {
            Debug.Assert(arguments.Length == 1);

            CodeValue argValue = (CodeValue)arguments[0];

            operand = Operand.Create(argValue.Object);
        }

        private static void GetOperands(CodeTerm[] arguments, out Operand lhs, out Operand rhs)
        {
            Debug.Assert(arguments.Length == 2);

            CodeValue argValue0 = (CodeValue)arguments[0];
            CodeValue argValue1 = (CodeValue)arguments[1];

            lhs = Operand.Create(argValue0.Object);
            rhs = Operand.Create(argValue1.Object);
        }

        #endregion

        private abstract class Operand
        {
            #region Constructors

            public static Operand Create(object value)
            {
                if (value is int)
                {
                    return new IntegerOperand((int)value);
                }
                if (value is double)
                {
                    return new DoubleOperand((double)value);
                }

                throw new ArgumentException("value");
            }

            #endregion

            #region Public Properties

            public virtual bool IsInteger
            {
                get { return false; }
            }

            public virtual bool IsDouble
            {
                get { return false; }
            }

            public abstract int AsInteger
            {
                get;
            }

            public abstract double AsDouble
            {
                get;
            }

            #endregion

            #region Public Methods

            public abstract object Negate();

            public abstract object Increment();

            public abstract object Decrement();

            public abstract object Add(Operand rhs);

            public abstract object Subtract(Operand rhs);

            public abstract object Multiply(Operand rhs);

            public abstract object Divide(Operand rhs);

            public abstract object IntegerDivide(Operand rhs);

            public abstract object Remainder(Operand rhs);

            public abstract object Modulo(Operand rhs);

            public abstract object BitwiseAnd(Operand rhs);

            public abstract object BitwiseOr(Operand rhs);

            public abstract object BitwiseXor(Operand rhs);

            public abstract object BitwiseNot();

            public abstract object ShiftLeft(Operand rhs);

            public abstract object BitwiseShiftRight(Operand rhs);

            public abstract object IntegerShiftRight(Operand rhs);

            public abstract object AbsoluteValue();

            public abstract object Sign();

            public abstract object Minimum(Operand rhs);

            public abstract object Maximum(Operand rhs);

            public abstract object Power(Operand rhs);

            public abstract object SquareRoot();

            public abstract object ArcTangent();

            public abstract object Cosine();

            public abstract object ArcCosine();

            public abstract object Sine();

            public abstract object ArcSine();

            public abstract object Exp();

            public abstract object Log();

            #endregion
        }

        private sealed class IntegerOperand : Operand
        {
            #region Fields

            private int m_value;

            #endregion

            #region Constructors

            public IntegerOperand(int value)
            {
                m_value = value;
            }

            #endregion

            #region Public Properties

            public override bool IsInteger
            {
                get { return true; }
            }

            public override int AsInteger
            {
                get { return m_value; }
            }

            public override double AsDouble
            {
                get { return m_value; }
            }

            #endregion

            #region Public Methods

            public override object Negate()
            {
                return -AsInteger;
            }

            public override object Increment()
            {
                return AsInteger + 1;
            }

            public override object Decrement()
            {
                return AsInteger - 1;
            }

            public override object Add(Operand rhs)
            {
                if (rhs.IsInteger) return AsInteger + rhs.AsInteger;
                if (rhs.IsDouble) return AsInteger + rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Subtract(Operand rhs)
            {
                if (rhs.IsInteger) return AsInteger - rhs.AsInteger;
                if (rhs.IsDouble) return AsInteger - rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Multiply(Operand rhs)
            {
                if (rhs.IsInteger) return AsInteger * rhs.AsInteger;
                if (rhs.IsDouble) return AsInteger * rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Divide(Operand rhs)
            {
                return AsDouble / rhs.AsDouble;
            }

            public override object IntegerDivide(Operand rhs)
            {
                return (int)Math.Round(AsDouble / rhs.AsDouble);
            }

            public override object Remainder(Operand rhs)
            {
                return AsInteger - (int)Math.Round(AsDouble / rhs.AsDouble) * rhs.AsInteger;
            }

            public override object Modulo(Operand rhs)
            {
                return AsInteger - (int)Math.Ceiling(AsDouble / rhs.AsDouble) * rhs.AsInteger;
            }

            public override object BitwiseAnd(Operand rhs)
            {
                return AsInteger & rhs.AsInteger;
            }

            public override object BitwiseOr(Operand rhs)
            {
                return AsInteger | rhs.AsInteger;
            }

            public override object BitwiseXor(Operand rhs)
            {
                return AsInteger ^ rhs.AsInteger;
            }

            public override object BitwiseNot()
            {
                return ~AsInteger;
            }

            public override object ShiftLeft(Operand rhs)
            {
                return AsInteger << rhs.AsInteger;
            }

            public override object BitwiseShiftRight(Operand rhs)
            {
                return (uint)AsInteger >> rhs.AsInteger;
            }

            public override object IntegerShiftRight(Operand rhs)
            {
                return AsInteger >> rhs.AsInteger;
            }

            public override object AbsoluteValue()
            {
                return Math.Abs(AsInteger);
            }

            public override object Sign()
            {
                int value = AsInteger;
                if (value < 0) return -1;
                if (value > 0) return 1;
                return 0;
            }

            public override object Minimum(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Min(AsInteger, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Min(AsInteger, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object Maximum(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Max(AsInteger, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Max(AsInteger, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object Power(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Pow(AsInteger, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Pow(AsInteger, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object SquareRoot()
            {
                return Math.Sqrt(AsInteger);
            }

            public override object ArcTangent()
            {
                return Math.Atan(AsInteger);
            }

            public override object Cosine()
            {
                return Math.Cos(AsInteger);
            }

            public override object ArcCosine()
            {
                return Math.Acos(AsInteger);
            }

            public override object Sine()
            {
                return Math.Sin(AsInteger);
            }

            public override object ArcSine()
            {
                return Math.Asin(AsInteger);
            }

            public override object Exp()
            {
                return Math.Exp(AsInteger);
            }

            public override object Log()
            {
                return Math.Log(AsInteger);
            }

            #endregion
        }

        private sealed class DoubleOperand : Operand
        {
            #region Fields

            private double m_value;

            #endregion

            #region Constructors

            public DoubleOperand(double value)
            {
                m_value = value;
            }

            #endregion

            #region Public Properties

            public override bool IsDouble
            {
                get { return true; }
            }

            public override int AsInteger
            {
                get { return (int)m_value; }
            }

            public override double AsDouble
            {
                get { return m_value; }
            }

            #endregion

            #region Public Methods

            public override object Negate()
            {
                return -AsDouble;
            }

            public override object Increment()
            {
                return AsDouble + 1;
            }

            public override object Decrement()
            {
                return AsDouble - 1;
            }

            public override object Add(Operand rhs)
            {
                if (rhs.IsInteger) return AsDouble + rhs.AsInteger;
                if (rhs.IsDouble) return AsDouble + rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Subtract(Operand rhs)
            {
                if (rhs.IsInteger) return AsDouble - rhs.AsInteger;
                if (rhs.IsDouble) return AsDouble - rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Multiply(Operand rhs)
            {
                if (rhs.IsInteger) return AsDouble * rhs.AsInteger;
                if (rhs.IsDouble) return AsDouble * rhs.AsDouble;
                throw new ArgumentException();
            }

            public override object Divide(Operand rhs)
            {
                return AsDouble / rhs.AsDouble;
            }

            public override object IntegerDivide(Operand rhs)
            {
                return (int)Math.Round(AsDouble / rhs.AsDouble);
            }

            public override object Remainder(Operand rhs)
            {
                return AsInteger - (int)Math.Round(AsDouble / rhs.AsDouble) * rhs.AsInteger;
            }

            public override object Modulo(Operand rhs)
            {
                return AsInteger - (int)Math.Ceiling(AsDouble / rhs.AsDouble) * rhs.AsInteger;
            }

            public override object BitwiseAnd(Operand rhs)
            {
                return AsInteger & rhs.AsInteger;
            }

            public override object BitwiseOr(Operand rhs)
            {
                return AsInteger | rhs.AsInteger;
            }

            public override object BitwiseXor(Operand rhs)
            {
                return AsInteger ^ rhs.AsInteger;
            }

            public override object BitwiseNot()
            {
                return ~AsInteger;
            }

            public override object ShiftLeft(Operand rhs)
            {
                return AsInteger << rhs.AsInteger;
            }

            public override object BitwiseShiftRight(Operand rhs)
            {
                return (uint)AsInteger >> rhs.AsInteger;
            }

            public override object IntegerShiftRight(Operand rhs)
            {
                return AsInteger >> rhs.AsInteger;
            }

            public override object AbsoluteValue()
            {
                return Math.Abs(AsDouble);
            }

            public override object Sign()
            {
                double value = AsDouble;
                if (value < 0) return -1;
                if (value > 0) return 1;
                return 0;
            }

            public override object Minimum(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Min(AsDouble, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Min(AsDouble, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object Maximum(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Max(AsDouble, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Max(AsDouble, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object Power(Operand rhs)
            {
                if (rhs.IsInteger) return Math.Pow(AsDouble, rhs.AsInteger);
                if (rhs.IsDouble) return Math.Pow(AsDouble, rhs.AsDouble);
                throw new ArgumentException();
            }

            public override object SquareRoot()
            {
                return Math.Sqrt(AsDouble);
            }

            public override object ArcTangent()
            {
                return Math.Atan(AsDouble);
            }

            public override object Cosine()
            {
                return Math.Cos(AsDouble);
            }

            public override object ArcCosine()
            {
                return Math.Acos(AsDouble);
            }

            public override object Sine()
            {
                return Math.Sin(AsDouble);
            }

            public override object ArcSine()
            {
                return Math.Asin(AsDouble);
            }

            public override object Exp()
            {
                return Math.Exp(AsDouble);
            }

            public override object Log()
            {
                return Math.Log(AsDouble);
            }

            #endregion
        }
    }
}
