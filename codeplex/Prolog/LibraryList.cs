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
    /// Represents a list of <see cref="Library"/> objects.
    /// </summary>
    public sealed class LibraryList : ReadableList<Library>
    {
        #region Constructors

        internal LibraryList(ObservableCollection<Library> libraries)
            : base(libraries)
        { }

        public static LibraryList Create()
        {
            return new LibraryList(new ObservableCollection<Library>());
        }

        #endregion

        #region Public Properties

        public LibraryMethod this[Functor functor]
        {
            get
            {
                if (functor == null)
                {
                    throw new ArgumentNullException("functor");
                }

                foreach (Library library in this)
                {
                    if (library.Contains(functor))
                    {
                        return library[functor];
                    }
                }

                throw new KeyNotFoundException();
            }
        }

        #endregion

        #region Public Members

        public void Add(Library library)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }
            if (Contains(library))
            {
                throw new ArgumentException("Item already exists.", "library");
            }

            Items.Add(library);
        }

        public void Remove(Library library)
        {
            if (library == null)
            {
                throw new ArgumentNullException("library");
            }
            if (!Contains(library))
            {
                throw new ArgumentException("Item not found.", "library");
            }

            Items.Remove(library);
        }

        public bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            foreach (Library library in this)
            {
                if (library.Contains(functor))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
