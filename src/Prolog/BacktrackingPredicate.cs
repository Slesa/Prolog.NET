/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class BacktrackingPredicate : LibraryMethod
    {
        readonly BacktrackingPredicateDelegate _backtrackingPredicateDelegate;

        internal BacktrackingPredicate(LibraryMethodList container, Functor functor, BacktrackingPredicateDelegate backtrackingPredicateDelegate)
            : base(container, functor, false)
        {
            if (backtrackingPredicateDelegate == null)
            {
                throw new ArgumentNullException("backtrackingPredicateDelegate");
            }
            _backtrackingPredicateDelegate = backtrackingPredicateDelegate;
        }

        public BacktrackingPredicateDelegate BacktrackingPredicateDelegate
        {
            get { return _backtrackingPredicateDelegate; }
        }
    }
}
