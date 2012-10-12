/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Prolog.Code;

namespace Prolog
{
    /// <summary>
    /// Represents a list of <see cref="LibraryMethod"/> objects.
    /// </summary>
    public sealed class LibraryMethodList : ReadableList<LibraryMethod>
    {
        #region Fields

        private Library m_library;

        #endregion

        #region Constructors

        internal LibraryMethodList(Library library, ObservableCollection<LibraryMethod> methods)
            : base(methods)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }

            m_library = library;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="Prolog.Library"/> containing this object.
        /// </summary>
        public Library Library
        {
            get { return m_library; }
        }

        /// <summary>
        /// Gets the <see cref="LibraryMethod"/> object associated with the specified <see cref="Functor"/>.
        /// </summary>
        /// <param name="functor">The <see cref="Functor"/> of the <see cref="LibraryMethod"/> to get.</param>
        /// <returns>The <see cref="LibraryMethod"/> with the specified <see cref="Functor"/>.</returns>
        public LibraryMethod this[Functor functor]
        {
            get
            {
                if (functor == null)
                {
                    throw new ArgumentNullException("functor");
                }

                foreach (LibraryMethod method in this)
                {
                    if (method.Functor == functor)
                    {
                        return method;
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Removes a <see cref="LibraryMethod"/> from the list.
        /// </summary>
        /// <param name="method">The <see cref="LibraryMethod"/> to remove from the list.</param>
        public void Remove(LibraryMethod method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (!Contains(method))
            {
                throw new ArgumentException("Item not found.", "method");
            }

            Items.Remove(method);
            Library.Touch();
        }

        #endregion

        #region Internal Members

        internal bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            foreach (LibraryMethod method in this)
            {
                if (method.Functor == functor)
                {
                    return true;
                }
            }

            return false;
        }

        internal Function Add(Functor functor, FunctionDelegate functionDelegate)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (functionDelegate == null)
            {
                throw new ArgumentNullException("functionDelegate");
            }
            if (Contains(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            Function function = new Function(this, functor, functionDelegate);
            Items.Add(function);
            Library.Touch();

            return function;
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
            if (Contains(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            Predicate predicate = new Predicate(this, functor, predicateDelegate, canEvaluate);
            Items.Add(predicate);
            Library.Touch();

            return predicate;
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
            if (Contains(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            BacktrackingPredicate predicate = new BacktrackingPredicate(this, functor, backtrackingPredicateDelegate);
            Items.Add(predicate);
            Library.Touch();

            return predicate;
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
            if (Contains(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            CodePredicate predicate = new CodePredicate(this, functor, codePredicateDelegate);
            Items.Add(predicate);
            Library.Touch();

            return predicate;
        }

        #endregion
    }
}
