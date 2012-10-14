/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    /// <summary>
    /// Represents a readable collection of objects that can be individually accessed by index. 
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public interface IReadableList<T> : IReadableCollection<T>
    {
        int IndexOf(T item);
        T this[int index] { get; }
    }
}
