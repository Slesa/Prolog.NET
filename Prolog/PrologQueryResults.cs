/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;

namespace Prolog
{
    /// <summary>
    /// Represents the query results returned by a <see cref="PrologMachine"/>.
    /// </summary>
    public sealed class PrologQueryResults : IPrologVariableListContainer
    {
        #region Fields

        private PrologVariableList m_variables;

        #endregion

        #region Constructors

        internal PrologQueryResults()
        {
            m_variables = new PrologVariableList(this);
        }

        #endregion

        #region Public Properties

        public PrologVariableList Variables
        {
            get { return m_variables; }
        }

        #endregion
    }
}
