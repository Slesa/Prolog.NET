/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Prolog
{
    /// <summary>
    /// Represents a Prolog predicate or operator whose behavior is implemented by a externally callable delegate.
    /// </summary>
    public abstract class LibraryMethod
    {
        #region Fields

        private LibraryMethodList m_container;
        private Functor m_functor;
        private bool m_canEvaluate;

        #endregion

        #region Constructors

        internal LibraryMethod(LibraryMethodList container, Functor functor, bool canEvaluate)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            m_container = container;
            m_functor = functor;
            m_canEvaluate = canEvaluate;
        }

        #endregion

        #region Public Properties

        public LibraryMethodList Container
        {
            get { return m_container; }
        }

        public Functor Functor
        {
            get { return m_functor; }
        }

        public bool CanEvaluate
        {
            get { return m_canEvaluate; }
        }

        #endregion
    }
}
