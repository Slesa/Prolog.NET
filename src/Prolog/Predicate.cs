/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class Predicate : LibraryMethod
    {
        internal Predicate(LibraryMethodList container, Functor functor, PredicateDelegate predicateDelegate, bool canEvaluate)
            : base(container, functor, canEvaluate)
        {
            if (predicateDelegate == null)
            {
                throw new ArgumentNullException("predicateDelegate");
            }

            PredicateDelegate = predicateDelegate;
        }

        public PredicateDelegate PredicateDelegate { get; private set; }
    }
}
