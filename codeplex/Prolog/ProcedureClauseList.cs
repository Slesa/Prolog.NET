/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.Collections.ObjectModel;

using Prolog.Code;

namespace Prolog
{
    public sealed class ProcedureClauseList : ReadableList<Clause>
    {
        #region Fields

        private Procedure m_procedure;

        #endregion

        #region Constructors

        internal ProcedureClauseList(Procedure procedure, ObservableCollection<Clause> clauses)
            : base(clauses)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException("procedure");
            }

            m_procedure = procedure;
        }

        #endregion

        #region Public Properties

        public Procedure Procedure
        {
            get { return m_procedure; }
        }

        #endregion

        #region Public Members

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

            int index = Items.IndexOf(clause);
            if (index > 0)
            {
                Move(index, index - 1);
                Procedure.InvalidateInstructionStream();
                Procedure.InvalidatePosition();
                Procedure.Container.Program.Touch();
            }
        }

        public void MoveDown(Clause clause)
        {
            if (!Items.Contains(clause))
            {
                throw new ArgumentException("Item not found.", "clause");
            }

            int index = Items.IndexOf(clause);
            if (index < Items.Count - 1)
            {
                Move(index, index + 1);
                Procedure.InvalidateInstructionStream();
                Procedure.InvalidatePosition();
                Procedure.Container.Program.Touch();
            }
        }

        #endregion

        #region Internal Members

        internal bool Contains(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            foreach (Clause clause in this)
            {
                if (clause.CodeSentence == codeSentence)
                {
                    return true;
                }
            }

            return false;
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

            Clause clause = new Clause(this, codeSentence);
            Items.Add(clause);
            Procedure.InvalidateInstructionStream();
            Procedure.InvalidatePosition();
            Procedure.Container.Program.Touch();

            return clause;
        }

        #endregion
    }
}
