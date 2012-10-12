/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;

namespace Prolog
{
    /// <summary>
    /// Contains state information passed to an <see cref="EventHandler"/> registered with a <see cref="PrologMachine"/>
    /// upon completion of a query.
    /// </summary>
    public sealed class PrologQueryEventArgs : EventArgs
    {
        #region Fields

        private static PrologQueryEventArgs s_empty = new PrologQueryEventArgs(null);

        private PrologQueryResults m_results;

        #endregion

        #region Constructors

        internal PrologQueryEventArgs(PrologQueryResults results)
        {
            m_results = results;
        }

        #endregion

        #region Public Properties

        public new static PrologQueryEventArgs Empty
        {
            get { return s_empty; }
        }

        public PrologQueryResults Results
        {
            get { return m_results; }
        }

        #endregion
    }
}
