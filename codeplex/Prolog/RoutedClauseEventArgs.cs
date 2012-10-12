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
        #region Fields

        private Clause m_clause;

        #endregion

        #region Constructors

        public RoutedClauseEventArgs(Clause clause, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            m_clause = clause;
        }

        public RoutedClauseEventArgs(Clause clause, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            m_clause = clause;
        }

        public RoutedClauseEventArgs(Clause clause)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }

            m_clause = clause;
        }

        #endregion

        #region Public Properties

        public Clause Clause
        {
            get { return m_clause; }
        }

        #endregion
    }
}
