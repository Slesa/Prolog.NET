/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    /// <summary>
    /// Represents a <see cref="LibraryMethod"/> implemented by a <see cref="FunctionDelegate"/>.
    /// </summary>
    public sealed class Function : LibraryMethod
    {
        internal Function(LibraryMethodList container, Functor functor, FunctionDelegate functionDelegate)
            : base(container, functor, true)
        {
            if (functionDelegate == null)
            {
                throw new ArgumentNullException("functionDelegate");
            }

            FunctionDelegate = functionDelegate;
        }

        public FunctionDelegate FunctionDelegate { get; private set; }
    }
}
