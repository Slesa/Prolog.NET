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
        #region Fields

        private static Library s_standard;

        private bool m_isModified;
        private LibraryMethodList m_methods;

        #endregion

        #region Constructors

        public Library()
        {
            m_isModified = false;
            m_methods = new LibraryMethodList(this, new ObservableCollection<LibraryMethod>());
        }

        #endregion

        #region Public Properties

        public static Library Standard
        {
            get
            {
                if (s_standard == null)
                {
                    Library standard = new Library();

                    // Term Unification and Evaluation
                    //
                    standard.Add("unify", @"=", 2, new PredicateDelegate(TermUnificationMethods.Unify), false);
                    standard.Add("can_unify", @"?=", 2, new PredicateDelegate(TermUnificationMethods.CanUnify), true);
                    standard.Add("cannot_unify", @"\=", 2, new PredicateDelegate(TermUnificationMethods.CannotUnify), true);
                    standard.Add("is", @":=", 2, new PredicateDelegate(TermUnificationMethods.Is), false);
                    standard.Add("assert", 1, new PredicateDelegate(TermUnificationMethods.Assert), false);
                    standard.Add("eval", 1, new CodePredicateDelegate(TermUnificationMethods.Eval));

                    // Control Constructs
                    //
                    standard.Add("true", 0, new PredicateDelegate(ControlConstructMethods.True), false);
                    standard.Add("fail", 0, new PredicateDelegate(ControlConstructMethods.Fail), false);
                    standard.Add("for", 3, new BacktrackingPredicateDelegate(ControlConstructMethods.For));

                    // All Solutions
                    //
                    standard.Add("findall", 3, new PredicateDelegate(AllSolutionMethods.FindAll), false);

                    // Type and Value Testing
                    //
                    standard.Add("var", 1, new PredicateDelegate(TypeValueTestingMethods.Var), true);
                    standard.Add("nonvar", 1, new PredicateDelegate(TypeValueTestingMethods.Nonvar), true);
                    standard.Add("atom", 1, new PredicateDelegate(TypeValueTestingMethods.Atom), true);
                    standard.Add("integer", 1, new PredicateDelegate(TypeValueTestingMethods.Integer), true);
                    standard.Add("float", 1, new PredicateDelegate(TypeValueTestingMethods.Float), true);
                    standard.Add("number", 1, new PredicateDelegate(TypeValueTestingMethods.Number), true);
                    standard.Add("atomic", 1, new PredicateDelegate(TypeValueTestingMethods.Atomic), true);
                    standard.Add("compound", 1, new PredicateDelegate(TypeValueTestingMethods.Compound), true);
                    standard.Add("callable", 1, new PredicateDelegate(TypeValueTestingMethods.Callable), true);
                    standard.Add("list", 1, new PredicateDelegate(TypeValueTestingMethods.List), true);
                    standard.Add("partial_list", 1, new PredicateDelegate(TypeValueTestingMethods.PartialList), true);
                    standard.Add("list_or_partial_list", 1, new PredicateDelegate(TypeValueTestingMethods.ListOrPartialList), true);
                    standard.Add("is_type", 2, new FunctionDelegate(TypeValueTestingMethods.IsType));
                    standard.Add("is_null", 1, new FunctionDelegate(TypeValueTestingMethods.IsNull));
                    standard.Add("is_empty", 1, new FunctionDelegate(TypeValueTestingMethods.IsEmpty));

                    // Term Processing
                    //
                    standard.Add("functor", 3, new PredicateDelegate(TermProcessingMethods.Functor), false);
                    standard.Add("arg", 3, new PredicateDelegate(TermProcessingMethods.Arg), false);
                    standard.Add("composed_of", @"=..", 2, new PredicateDelegate(TermProcessingMethods.ComposedOf), false);
                    standard.Add("copy_term", 2, new PredicateDelegate(TermProcessingMethods.CopyTerm), false);

                    // Type Conversion Expressions
                    //
                    standard.Add("get_type", 1, new FunctionDelegate(TypeConversionExpressionMethods.GetType));
                    standard.Add("type_of", 1, new FunctionDelegate(TypeConversionExpressionMethods.TypeOf));
                    standard.Add("to_integer", 1, new FunctionDelegate(TypeConversionExpressionMethods.ToInteger));
                    standard.Add("to_double", 1, new FunctionDelegate(TypeConversionExpressionMethods.ToDouble));
                    standard.Add("to_string", 1, new FunctionDelegate(TypeConversionExpressionMethods.ToString));
                    standard.Add("to_string", 2, new FunctionDelegate(TypeConversionExpressionMethods.ToString));
                    standard.Add("to_date", 1, new FunctionDelegate(TypeConversionExpressionMethods.ToDate));
                    standard.Add("to_date", 3, new FunctionDelegate(TypeConversionExpressionMethods.ToDate));
                    standard.Add("to_boolean", 1, new FunctionDelegate(TypeConversionExpressionMethods.ToBoolean));
                    standard.Add("ceiling", 1, new FunctionDelegate(TypeConversionExpressionMethods.Ceiling));
                    standard.Add("floor", 1, new FunctionDelegate(TypeConversionExpressionMethods.Floor));
                    standard.Add("round", 1, new FunctionDelegate(TypeConversionExpressionMethods.Round));
                    standard.Add("truncate", 1, new FunctionDelegate(TypeConversionExpressionMethods.Truncate));

                    // Arithmetic Expressions
                    //
                    standard.Add("negate", "-",1, new FunctionDelegate(ArithmeticExpressionMethods.Negate));
                    standard.Add("inc", 1, new FunctionDelegate(ArithmeticExpressionMethods.Increment));
                    standard.Add("dec", 1, new FunctionDelegate(ArithmeticExpressionMethods.Decrement));
                    standard.Add("add", @"+", 2, new FunctionDelegate(ArithmeticExpressionMethods.Add));
                    standard.Add("subtract", @"-", 2, new FunctionDelegate(ArithmeticExpressionMethods.Subtract));
                    standard.Add("multiply", @"*", 2, new FunctionDelegate(ArithmeticExpressionMethods.Multiply));
                    standard.Add("divide", @"/", 2, new FunctionDelegate(ArithmeticExpressionMethods.Divide));
                    standard.Add("integer_divide", @"//", 2, new FunctionDelegate(ArithmeticExpressionMethods.IntegerDivide));
                    standard.Add("rem", 2, new FunctionDelegate(ArithmeticExpressionMethods.Remainder));
                    standard.Add("mod", 2, new FunctionDelegate(ArithmeticExpressionMethods.Modulo));
                    standard.Add("bitwise_and", @"/\", 2, new FunctionDelegate(ArithmeticExpressionMethods.BitwiseAnd));
                    standard.Add("bitwise_or", @"\/", 2, new FunctionDelegate(ArithmeticExpressionMethods.BitwiseOr));
                    standard.Add("bitwise_xor", @"^", 2, new FunctionDelegate(ArithmeticExpressionMethods.BitwiseXor));
                    standard.Add("bitwise_not", @"\", 1, new FunctionDelegate(ArithmeticExpressionMethods.BitwiseNot));
                    standard.Add("shift_left", @"<<", 2, new FunctionDelegate(ArithmeticExpressionMethods.ShiftLeft));
                    standard.Add("integer_shift_right", @">>", 2, new FunctionDelegate(ArithmeticExpressionMethods.IntegerShiftRight));
                    standard.Add("bitwise_shift_right", 2, new FunctionDelegate(ArithmeticExpressionMethods.BitwiseShiftRight));
                    standard.Add("abs", 1, new FunctionDelegate(ArithmeticExpressionMethods.AbsoluteValue));
                    standard.Add("sign", 1, new FunctionDelegate(ArithmeticExpressionMethods.Sign));
                    standard.Add("min", 2, new FunctionDelegate(ArithmeticExpressionMethods.Minimum));
                    standard.Add("max", 2, new FunctionDelegate(ArithmeticExpressionMethods.Maximum));
                    standard.Add("power", 2, new FunctionDelegate(ArithmeticExpressionMethods.Power));
                    standard.Add("sqrt", 1, new FunctionDelegate(ArithmeticExpressionMethods.SquareRoot));
                    standard.Add("atan", 1, new FunctionDelegate(ArithmeticExpressionMethods.ArcTangent));
                    standard.Add("cos", 1, new FunctionDelegate(ArithmeticExpressionMethods.Cosign));
                    standard.Add("acos", 1, new FunctionDelegate(ArithmeticExpressionMethods.ArcCosign));
                    standard.Add("sin", 1, new FunctionDelegate(ArithmeticExpressionMethods.Sine));
                    standard.Add("asin", 1, new FunctionDelegate(ArithmeticExpressionMethods.ArcSine));
                    standard.Add("exp", 1, new FunctionDelegate(ArithmeticExpressionMethods.Exp));
                    standard.Add("log", 1, new FunctionDelegate(ArithmeticExpressionMethods.Log));

                    // String Expressions
                    //
                    standard.Add("substring", 2, new FunctionDelegate(StringExpressionMethods.Substring));
                    standard.Add("substring", 3, new FunctionDelegate(StringExpressionMethods.Substring));
                    standard.Add("length", 1, new FunctionDelegate(StringExpressionMethods.StringLength));
                    standard.Add("contains", 2, new FunctionDelegate(StringExpressionMethods.StringContains));
                    standard.Add("replace", 3, new FunctionDelegate(StringExpressionMethods.StringReplace));

                    // Value Comparison
                    //
                    standard.Add("equal", @"=:=", 2, new FunctionDelegate(ValueComparisonMethods.Equal));
                    standard.Add("unequal", @"=\=", 2, new FunctionDelegate(ValueComparisonMethods.Unequal));
                    standard.Add("less", @"<", 2, new FunctionDelegate(ValueComparisonMethods.Less));
                    standard.Add("less_equal", @"=<", 2, new FunctionDelegate(ValueComparisonMethods.LessEqual));
                    standard.Add("greater", @">", 2, new FunctionDelegate(ValueComparisonMethods.Greater));
                    standard.Add("greater_equal", @">=", 2, new FunctionDelegate(ValueComparisonMethods.GreaterEqual));

                    // Random Numbers
                    //
                    standard.Add("randomize", 0, new PredicateDelegate(RandomNumberMethods.Randomize), false);
                    standard.Add("set_seed", 1, new PredicateDelegate(RandomNumberMethods.SetSeed), false);
                    standard.Add("get_seed", 1, new PredicateDelegate(RandomNumberMethods.GetSeed), false);
                    standard.Add("random", 1, new PredicateDelegate(RandomNumberMethods.NextDouble), false);
                    standard.Add("random", 3, new PredicateDelegate(RandomNumberMethods.Next), false);

                    s_standard = standard;
                }

                return s_standard;
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
            get { return m_isModified; }
            private set
            {
                if (value != m_isModified)
                {
                    m_isModified = value;
                    RaisePropertyChanged(new PropertyChangedEventArgs("IsModified"));
                }
            }
        }

        public LibraryMethodList Methods
        {
            get { return m_methods; }
        }

        #endregion

        #region Public Methods

        public bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            if (Methods.Contains(functor))
            {
                return true;
            }

            return false;
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

            Function function = Methods.Add(functor, functionDelegate);

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

            Predicate predicate = Methods.Add(functor, predicateDelegate, canEvaluate);

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

            BacktrackingPredicate backtrackingPredicate = Methods.Add(functor, backtrackingPredicateDelegate);

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

            CodePredicate codePredicate = Methods.Add(functor, codePredicateDelegate);

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

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
