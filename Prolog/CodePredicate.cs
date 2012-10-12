/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class CodePredicate : LibraryMethod
    {
        #region Fields

        private CodePredicateDelegate m_codePredicateDelegate;

        #endregion

        #region Constructors

        internal CodePredicate(LibraryMethodList container, Functor functor, CodePredicateDelegate codePredicateDelegate)
            : base(container, functor, false)
        {
            if (codePredicateDelegate == null)
            {
                throw new ArgumentNullException("codePredicateDelegate");
            }

            m_codePredicateDelegate = codePredicateDelegate;
        }

        #endregion

        #region Public Properties

        public CodePredicateDelegate CodePredicateDelegate
        {
            get { return m_codePredicateDelegate; }
        }

        #endregion
    }
}
