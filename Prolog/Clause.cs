/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;
using System.ComponentModel;

using Prolog.Code;

namespace Prolog
{
    public sealed class Clause : IPrologInstructionStreamContainer, INotifyPropertyChanged
    {
        #region Fields

        private ProcedureClauseList m_container;
        private CodeSentence m_codeSentence;

        private WamInstructionStream m_wamInstructionStream;
        private PrologInstructionStream m_prologInstructionStream;

        #endregion

        #region Constructors

        internal Clause(ProcedureClauseList container, CodeSentence codeSentence)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            m_container = container;
            m_codeSentence = codeSentence;
        }

        #endregion

        #region Public Properties

        public ProcedureClauseList Container
        {
            get { return m_container; }
        }

        public CodeSentence CodeSentence
        {
            get { return m_codeSentence; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Head == null
                    || value.Head.Functor != m_codeSentence.Head.Functor)
                {
                    throw new ArgumentException("Code specified for different procedure.");
                }

                m_codeSentence = value;
                RaisePropertyChanged(new PropertyChangedEventArgs("CodeSentence"));

                InvalidateInstructionStream();
                Container.Procedure.Container.Program.Touch();
            }
        }

        public int Index
        {
            get
            {
                return Container.IndexOf(this);
            }
        }

        public bool IsFirst
        {
            get
            {
                return Index == 0;
            }
        }

        public bool IsLast
        {
            get
            {
                return Index == Container.Count - 1;
            }
        }

        public PrologInstructionStream PrologInstructionStream
        {
            get
            {
                if (m_prologInstructionStream == null)
                {
                    m_prologInstructionStream = new PrologInstructionStream(this);
                }

                return m_prologInstructionStream;
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{0}.", CodeSentence);
        }

        #endregion

        #region IPrologInstructionStreamContainer Members

        WamInstructionStream IPrologInstructionStreamContainer.WamInstructionStream
        {
            get { return WamInstructionStream; }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Internal Members

        internal WamInstructionStream WamInstructionStream
        {
            get
            {
                if (m_wamInstructionStream == null)
                {
                    Compiler compiler = new Compiler();
                    m_wamInstructionStream = compiler.Compile(
                        CodeSentence, 
                        Container.Procedure.Functor, 
                        Index, 
                        IsLast, 
                        Container.Procedure.Container.Program.Libraries,
                        Container.Procedure.Container.Program.IsOptimized);
                }

                return m_wamInstructionStream;
            }
        }

        internal void InvalidatePosition()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs("IsFirst"));
            RaisePropertyChanged(new PropertyChangedEventArgs("IsLast"));
        }

        internal void InvalidateInstructionStream()
        {
            m_wamInstructionStream = null;

            if (m_prologInstructionStream != null)
            {
                m_prologInstructionStream = null;
                RaisePropertyChanged(new PropertyChangedEventArgs("PrologInstructionStream"));
            }
        }

        #endregion

        #region Hidden Members

        private void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        #endregion
    }
}
