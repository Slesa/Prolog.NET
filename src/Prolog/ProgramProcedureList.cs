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
        readonly Dictionary<Functor, Procedure> _procedureIndex;

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

            Program = program;
            _procedureIndex = new Dictionary<Functor, Procedure>();
        }

        public Program Program { get; private set; }

        public bool Contains(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            return _procedureIndex.ContainsKey(functor);
        }

        public Procedure this[Functor functor]
        {
            get { return _procedureIndex[functor]; }
        }

        public bool TryGetProcedure(Functor functor, out Procedure procedure)
        {
            return _procedureIndex.TryGetValue(functor, out procedure);
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
            Debug.Assert(_procedureIndex.ContainsKey(procedure.Functor));

            Items.Remove(procedure);
            if (_procedureIndex.ContainsKey(procedure.Functor))
            {
                _procedureIndex.Remove(procedure.Functor);
            }

            Program.Touch();
        }

        internal Procedure Add(Functor functor)
        {
            if (functor == null)
            {
                throw new ArgumentNullException("functor");
            }
            if (_procedureIndex.ContainsKey(functor))
            {
                throw new ArgumentException("Item already exists.", "functor");
            }

            var procedure = new Procedure(this, functor);
            Items.Add(procedure);
            _procedureIndex.Add(functor, procedure);

            Program.Touch();

            return procedure;
        }
    }
}
