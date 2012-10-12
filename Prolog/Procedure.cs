/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;

namespace Prolog
{
    /// <summary>
    /// Represents a collection of <see cref="Clause"/> objects in a <see cref="Program"/> which all share the same <see cref="Functor"/>.
    /// </summary>
    public sealed class Procedure
    {
        #region Fields

        private ProgramProcedureList m_container;
        private Functor m_functor;
        private ProcedureClauseList m_clauses;
        
        #endregion

        #region Constructors

        internal Procedure(ProgramProcedureList container, Functor functor)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            m_container = container;
            m_functor = functor;
            m_clauses = new ProcedureClauseList(this, new ObservableCollection<Clause>());
        }

        #endregion

        #region Public Properties

        public ProgramProcedureList Container
        {
            get { return m_container; }
        }

        public Functor Functor
        {
            get { return m_functor; }
        }

        public ProcedureClauseList Clauses
        {
            get { return m_clauses; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{0}", Functor);
        }

        #endregion

        #region Internal Methods

        internal void InvalidateInstructionStream()
        {
            foreach (Clause clause in Clauses)
            {
                clause.InvalidateInstructionStream();
            }
        }

        internal void InvalidatePosition()
        {
            foreach (Clause clause in Clauses)
            {
                clause.InvalidatePosition();
            }
        }

        #endregion
    }
}
