/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class CodePredicate : LibraryMethod
    {
        private readonly CodePredicateDelegate _codePredicateDelegate;

        internal CodePredicate(LibraryMethodList container, Functor functor, CodePredicateDelegate codePredicateDelegate)
            : base(container, functor, false)
        {
            if (codePredicateDelegate == null)
            {
                throw new ArgumentNullException("codePredicateDelegate");
            }

            _codePredicateDelegate = codePredicateDelegate;
        }

        public CodePredicateDelegate CodePredicateDelegate
        {
            get { return _codePredicateDelegate; }
        }
    }
}
