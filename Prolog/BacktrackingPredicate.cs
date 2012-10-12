/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class BacktrackingPredicate : LibraryMethod
    {
        #region Fields

        private BacktrackingPredicateDelegate m_backtrackingPredicateDelegate;

        #endregion

        #region Constructors

        internal BacktrackingPredicate(LibraryMethodList container, Functor functor, BacktrackingPredicateDelegate backtrackingPredicateDelegate)
            : base(container, functor, false)
        {
            if (backtrackingPredicateDelegate == null)
            {
                throw new ArgumentNullException("backtrackingPredicateDelegate");
            }

            m_backtrackingPredicateDelegate = backtrackingPredicateDelegate;
        }

        #endregion

        #region Public Properties

        public BacktrackingPredicateDelegate BacktrackingPredicateDelegate
        {
            get { return m_backtrackingPredicateDelegate; }
        }

        #endregion
    }
}
