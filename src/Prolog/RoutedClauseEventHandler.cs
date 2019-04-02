#if WPFCODE
/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    /// <summary>
    /// Represents the method that handle routed events associated with <see cref="Clause"/> objects.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void RoutedClauseEventHandler(object sender, RoutedClauseEventArgs e);
}
#endif