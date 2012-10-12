/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Prolog
{
    public sealed class ProgramProcedureList : ReadableList<Procedure>
    {
        #region Fields

        private Program m_program;
        private Dictionary<Functor, Procedure> m_procedureIndex;

        #endregion

        #region Constructors

        internal ProgramProcedureList(Program program, ObservableCollection<Procedure> procedures)
            : base(procedures)
        {
            if (program == null)
            {
                throw new ArgumentNullException("program");
            }
            if (procedures == null)
            {
                throw new ArgumentNullException("procedures");
            }

            m_program = program;
            m_procedureIndex = new Dictionary<Functor, Procedure>();
        }

        #endregion

        #region Public Properties

        public Program Program
        {
            get { return m_program; }
        }

        #endregion

        #region Public Members

        public bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }

            return m_procedureIndex.ContainsKey(functor);
        }

        public Procedure this[Functor functor]
        {
            get { return m_procedureIndex[functor]; }
        }

        public bool TryGetProcedure(Functor functor, out Procedure procedure)
        {
            return m_procedureIndex.TryGetValue(functor, out procedure);
        }

        public void Remove(Procedure procedure)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException("procedure");
            }
            if (!Items.Contains(procedure))
            {
                throw new ArgumentException("Item not found.", "procedure");
            }
            Debug.Assert(m_procedureIndex.ContainsKey(procedure.Functor));

            Items.Remove(procedure);
            if (m_procedureIndex.ContainsKey(procedure.Functor))
            {
                m_procedureIndex.Remove(procedure.Functor);
            }

            Program.Touch();
        }

        #endregion

        #region Internal Members

        internal Procedure Add(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (m_procedureIndex.ContainsKey(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            Procedure procedure = new Procedure(this, functor);
            Items.Add(procedure);
            m_procedureIndex.Add(functor, procedure);

            Program.Touch();

            return procedure;
        }

        #endregion
    }
}
