/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    /// <summary>
    /// Represents a Prolog predicate or operator whose behavior is implemented by a externally callable delegate.
    /// </summary>
    public abstract class LibraryMethod
    {
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

            Container = container;
            Functor = functor;
            CanEvaluate = canEvaluate;
        }

        public LibraryMethodList Container { get; private set; }
        public Functor Functor { get; private set; }
        public bool CanEvaluate { get; private set; }
    }
}
