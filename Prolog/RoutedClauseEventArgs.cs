/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Windows;

namespace Prolog
{
    /// <summary>
    /// Contains state information passed to a <see cref="RoutedClauseEventHandler"/>.
    /// </summary>
    public class RoutedClauseEventArgs : RoutedEventArgs
    {
        public RoutedClauseEventArgs(Clause clause, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            Clause = clause;
        }

        public RoutedClauseEventArgs(Clause clause, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            Clause = clause;
        }

        public RoutedClauseEventArgs(Clause clause)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            Clause = clause;
        }

        public Clause Clause { get; private set; }
    }
}
