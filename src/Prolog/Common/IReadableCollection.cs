/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    /// <summary>
    /// Represents a readable collection of objects. 
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public interface IReadableCollection<T> : IEnumerable<T>
    {
        int Count { get; }
        bool Contains(T item);
    }
}
