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
            Container = container;
            Functor = functor;
            Clauses = new ProcedureClauseList(this, new ObservableCollection<Clause>());
        }

        public ProgramProcedureList Container { get; private set; }
        public Functor Functor { get; private set; }
        public ProcedureClauseList Clauses { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}", Functor);
        }

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
    }
}
