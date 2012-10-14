/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Prolog
{
    /// <summary>
    /// Represents a collection of predicates and operators whose behavior is implemented by externally callable delegates.
    /// </summary>
    public sealed class Library : INotifyPropertyChanged
    {
        static Library _standard;
        bool _isModified;

        public Library()
        {
            _isModified = false;
            Methods = new LibraryMethodList(this, new ObservableCollection<LibraryMethod>());
        }

        public static Library Standard
        {
            get
            {
                if (_standard == null)
                {
                    var standard = new Library();

                    // Term Unification and Evaluation
                    //
                    standard.Add("unify", @"=", 2, TermUnificationMethods.Unify, false);
                    standard.Add("can_unify", @"?=", 2, TermUnificationMethods.CanUnify, true);
                    standard.Add("cannot_unify", @"\=", 2, TermUnificationMethods.CannotUnify, true);
                    standard.Add("is", @":=", 2, TermUnificationMethods.Is, false);
                    standard.Add("assert", 1, TermUnificationMethods.Assert, false);
                    standard.Add("eval", 1, new CodePredicateDelegate(TermUnificationMethods.Eval));

                    // Control Constructs
                    //
                    standard.Add("true", 0, ControlConstructMethods.True, false);
                    standard.Add("fail", 0, ControlConstructMethods.Fail, false);
                    standard.Add("for", 3, new BacktrackingPredicateDelegate(ControlConstructMethods.For));

                    // All Solutions
                    //
                    standard.Add("findall", 3, AllSolutionMethods.FindAll, false);

                    // Type and Value Testing
                    //
                    standard.Add("var", 1, TypeValueTestingMethods.Var, true);
                    standard.Add("nonvar", 1, TypeValueTestingMethods.Nonvar, true);
                    standard.Add("atom", 1, TypeValueTestingMethods.Atom, true);
                    standard.Add("integer", 1, TypeValueTestingMethods.Integer, true);
                    standard.Add("float", 1, TypeValueTestingMethods.Float, true);
                    standard.Add("number", 1, TypeValueTestingMethods.Number, true);
                    standard.Add("atomic", 1, TypeValueTestingMethods.Atomic, true);
                    standard.Add("compound", 1, TypeValueTestingMethods.Compound, true);
                    standard.Add("callable", 1, TypeValueTestingMethods.Callable, true);
                    standard.Add("list", 1, TypeValueTestingMethods.List, true);
                    standard.Add("partial_list", 1, TypeValueTestingMethods.PartialList, true);
                    standard.Add("list_or_partial_list", 1, TypeValueTestingMethods.ListOrPartialList, true);
                    standard.Add("is_type", 2, TypeValueTestingMethods.IsType);
                    standard.Add("is_null", 1, TypeValueTestingMethods.IsNull);
                    standard.Add("is_empty", 1, TypeValueTestingMethods.IsEmpty);

                    // Term Processing
                    //
                    standard.Add("functor", 3, TermProcessingMethods.Functor, false);
                    standard.Add("arg", 3, TermProcessingMethods.Arg, false);
                    standard.Add("composed_of", @"=..", 2, TermProcessingMethods.ComposedOf, false);
                    standard.Add("copy_term", 2, TermProcessingMethods.CopyTerm, false);

                    // Type Conversion Expressions
                    //
                    standard.Add("get_type", 1, TypeConversionExpressionMethods.GetType);
                    standard.Add("type_of", 1, TypeConversionExpressionMethods.TypeOf);
                    standard.Add("to_integer", 1, TypeConversionExpressionMethods.ToInteger);
                    standard.Add("to_double", 1, TypeConversionExpressionMethods.ToDouble);
                    standard.Add("to_string", 1, TypeConversionExpressionMethods.ToString);
                    standard.Add("to_string", 2, TypeConversionExpressionMethods.ToString);
                    standard.Add("to_date", 1, TypeConversionExpressionMethods.ToDate);
                    standard.Add("to_date", 3, TypeConversionExpressionMethods.ToDate);
                    standard.Add("to_boolean", 1, TypeConversionExpressionMethods.ToBoolean);
                    standard.Add("ceiling", 1, TypeConversionExpressionMethods.Ceiling);
                    standard.Add("floor", 1, TypeConversionExpressionMethods.Floor);
                    standard.Add("round", 1, TypeConversionExpressionMethods.Round);
                    standard.Add("truncate", 1, TypeConversionExpressionMethods.Truncate);

                    // Arithmetic Expressions
                    //
                    standard.Add("negate", "-",1, ArithmeticExpressionMethods.Negate);
                    standard.Add("inc", 1, ArithmeticExpressionMethods.Increment);
                    standard.Add("dec", 1, ArithmeticExpressionMethods.Decrement);
                    standard.Add("add", @"+", 2, ArithmeticExpressionMethods.Add);
                    standard.Add("subtract", @"-", 2, ArithmeticExpressionMethods.Subtract);
                    standard.Add("multiply", @"*", 2, ArithmeticExpressionMethods.Multiply);
                    standard.Add("divide", @"/", 2, ArithmeticExpressionMethods.Divide);
                    standard.Add("integer_divide", @"//", 2, ArithmeticExpressionMethods.IntegerDivide);
                    standard.Add("rem", 2, ArithmeticExpressionMethods.Remainder);
                    standard.Add("mod", 2, ArithmeticExpressionMethods.Modulo);
                    standard.Add("bitwise_and", @"/\", 2, ArithmeticExpressionMethods.BitwiseAnd);
                    standard.Add("bitwise_or", @"\/", 2, ArithmeticExpressionMethods.BitwiseOr);
                    standard.Add("bitwise_xor", @"^", 2, ArithmeticExpressionMethods.BitwiseXor);
                    standard.Add("bitwise_not", @"\", 1, ArithmeticExpressionMethods.BitwiseNot);
                    standard.Add("shift_left", @"<<", 2, ArithmeticExpressionMethods.ShiftLeft);
                    standard.Add("integer_shift_right", @">>", 2, ArithmeticExpressionMethods.IntegerShiftRight);
                    standard.Add("bitwise_shift_right", 2, ArithmeticExpressionMethods.BitwiseShiftRight);
                    standard.Add("abs", 1, ArithmeticExpressionMethods.AbsoluteValue);
                    standard.Add("sign", 1, ArithmeticExpressionMethods.Sign);
                    standard.Add("min", 2, ArithmeticExpressionMethods.Minimum);
                    standard.Add("max", 2, ArithmeticExpressionMethods.Maximum);
                    standard.Add("power", 2, ArithmeticExpressionMethods.Power);
                    standard.Add("sqrt", 1, ArithmeticExpressionMethods.SquareRoot);
                    standard.Add("atan", 1, ArithmeticExpressionMethods.ArcTangent);
                    standard.Add("cos", 1, ArithmeticExpressionMethods.Cosign);
                    standard.Add("acos", 1, ArithmeticExpressionMethods.ArcCosign);
                    standard.Add("sin", 1, ArithmeticExpressionMethods.Sine);
                    standard.Add("asin", 1, ArithmeticExpressionMethods.ArcSine);
                    standard.Add("exp", 1, ArithmeticExpressionMethods.Exp);
                    standard.Add("log", 1, ArithmeticExpressionMethods.Log);

                    // String Expressions
                    //
                    standard.Add("substring", 2, StringExpressionMethods.Substring);
                    standard.Add("substring", 3, StringExpressionMethods.Substring);
                    standard.Add("length", 1, StringExpressionMethods.StringLength);
                    standard.Add("contains", 2, StringExpressionMethods.StringContains);
                    standard.Add("replace", 3, StringExpressionMethods.StringReplace);

                    // Value Comparison
                    //
                    standard.Add("equal", @"=:=", 2, ValueComparisonMethods.Equal);
                    standard.Add("unequal", @"=\=", 2, ValueComparisonMethods.Unequal);
                    standard.Add("less", @"<", 2, ValueComparisonMethods.Less);
                    standard.Add("less_equal", @"=<", 2, ValueComparisonMethods.LessEqual);
                    standard.Add("greater", @">", 2, ValueComparisonMethods.Greater);
                    standard.Add("greater_equal", @">=", 2, ValueComparisonMethods.GreaterEqual);

                    // Random Numbers
                    //
                    standard.Add("randomize", 0, RandomNumberMethods.Randomize, false);
                    standard.Add("set_seed", 1, RandomNumberMethods.SetSeed, false);
                    standard.Add("get_seed", 1, RandomNumberMethods.GetSeed, false);
                    standard.Add("random", 1, RandomNumberMethods.NextDouble, false);
                    standard.Add("random", 3, RandomNumberMethods.Next, false);

                    _standard = standard;
                }

                return _standard;
            }
        }

        public LibraryMethod this[Functor functor]
        {
            get
            {
                if (functor == null)
                {
                    throw new ArgumentNullException("functor");
                }
                return Methods[functor];
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
            private set
            {
                if (value != _isModified)
                {
                    _isModified = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsModified"));
                }
            }
        }

        public LibraryMethodList Methods { get; private set; }

        public bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            return Methods.Contains(functor);
        }

        public Function Add(Functor functor, FunctionDelegate functionDelegate)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (functionDelegate == null)
            {
                throw new ArgumentNullException("functionDelegate");
            }
            var function = Methods.Add(functor, functionDelegate);
            return function;
        }

        public void Add(string name, int arity, FunctionDelegate functionDelegate)
        {
            Add(new Functor(name, arity), functionDelegate);
        }

        public void Add(string name, string op, int arity, FunctionDelegate functionDelegate)
        {
            Add(new Functor(name, arity), functionDelegate);
            Add(new Functor(op, arity), functionDelegate);
        }

        internal Predicate Add(Functor functor, PredicateDelegate predicateDelegate, bool canEvaluate)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (predicateDelegate == null)
            {
                throw new ArgumentNullException("predicateDelegate");
            }
            var predicate = Methods.Add(functor, predicateDelegate, canEvaluate);
            return predicate;
        }

        internal void Add(string name, int arity, CodePredicateDelegate codePredicateDelegate)
        {
            Add(new Functor(name, arity), codePredicateDelegate);
        }

        internal void Add(string name, int arity, PredicateDelegate predicateDelegate, bool canEvaluate)
        {
            Add(new Functor(name, arity), predicateDelegate, canEvaluate);
        }

        internal void Add(string name, string op, int arity, PredicateDelegate predicateDelegate, bool canEvaluate)
        {
            Add(new Functor(name, arity), predicateDelegate, canEvaluate);
            Add(new Functor(op, arity), predicateDelegate, canEvaluate);
        }

        internal BacktrackingPredicate Add(Functor functor, BacktrackingPredicateDelegate backtrackingPredicateDelegate)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (backtrackingPredicateDelegate == null)
            {
                throw new ArgumentNullException("backtrackingPredicateDelegate");
            }
            var backtrackingPredicate = Methods.Add(functor, backtrackingPredicateDelegate);
            return backtrackingPredicate;
        }

        internal CodePredicate Add(Functor functor, CodePredicateDelegate codePredicateDelegate)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (codePredicateDelegate == null)
            {
                throw new ArgumentNullException("codePredicateDelegate");
            }
            var codePredicate = Methods.Add(functor, codePredicateDelegate);
            return codePredicate;
        }

        internal void Add(string name, int arity, BacktrackingPredicateDelegate backtrackingPredicateDelegate)
        {
            Add(new Functor(name, arity), backtrackingPredicateDelegate);
        }

        public void Touch()
        {
            IsModified = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
