/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class Predicate : LibraryMethod
    {
        #region Fields

        private PredicateDelegate m_predicateDelegate;

        #endregion

        #region Constructors

        internal Predicate(LibraryMethodList container, Functor functor, PredicateDelegate predicateDelegate, bool canEvaluate)
            : base(container, functor, canEvaluate)
        {
            if (predicateDelegate == null)
            {
                throw new ArgumentNullException("predicateDelegate");
            }

            m_predicateDelegate = predicateDelegate;
        }

        #endregion

        #region Public Properties

        public PredicateDelegate PredicateDelegate
        {
            get { return m_predicateDelegate; }
        }

        #endregion
    }
}
