/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prolog.Code;

namespace Prolog
{
    public sealed class ProcedureClauseList : ReadableList<Clause>
    {
        internal ProcedureClauseList(Procedure procedure, ObservableCollection<Clause> clauses)
            : base(clauses)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException("procedure");
            }

            Procedure = procedure;
        }

        public Procedure Procedure { get; private set; }

        public void Remove(Clause clause)
        {
            if (clause == null)
            {
                throw new ArgumentNullException("clause");
            }
            if (!Contains(clause))
            {
                throw new ArgumentException("Item not found.", "clause");
            }
            Items.Remove(clause);
            if (Items.Count == 0)
            {
                Procedure.Container.Remove(Procedure);
            }
            else
            {
                Procedure.InvalidateInstructionStream();
                Procedure.InvalidatePosition();
                Procedure.Container.Program.Touch();
            }
        }

        public void MoveUp(Clause clause)
        {
            if (!Items.Contains(clause))
            {
                throw new ArgumentException("Item not found.", "clause");
            }

            var index = Items.IndexOf(clause);
            if (index <= 0) return;
            
            Move(index, index - 1);
            Procedure.InvalidateInstructionStream();
            Procedure.InvalidatePosition();
            Procedure.Container.Program.Touch();
        }

        public void MoveDown(Clause clause)
        {
            if (!Items.Contains(clause))
            {
                throw new ArgumentException("Item not found.", "clause");
            }

            var index = Items.IndexOf(clause);
            if (index >= Items.Count - 1) return;

            Move(index, index + 1);
            Procedure.InvalidateInstructionStream();
            Procedure.InvalidatePosition();
            Procedure.Container.Program.Touch();
        }

        internal bool Contains(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            return this.Any(clause => clause.CodeSentence == codeSentence);
        }

        internal Clause Add(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }
            if (codeSentence.Head == null)
            {
                throw new ArgumentException("Query cannot be added to program.", "codeSentence");
            }
            if (Functor.Create(codeSentence.Head.Functor) != Procedure.Functor)
            {
                throw new ArgumentException("Clause not member of procedure.", "codeSentence");
            }
            if (Contains(codeSentence))
            {
                throw new ArgumentException("Item already exists.", "codeSentence");
            }

            var clause = new Clause(this, codeSentence);
            Items.Add(clause);
            Procedure.InvalidateInstructionStream();
            Procedure.InvalidatePosition();
            Procedure.Container.Program.Touch();

            return clause;
        }
    }
}
