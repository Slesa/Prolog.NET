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
        CodeSentence _codeSentence;
        WamInstructionStream _wamInstructionStream;
        PrologInstructionStream _prologInstructionStream;

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

            Container = container;
            _codeSentence = codeSentence;
        }

        public ProcedureClauseList Container { get; private set; }

        public CodeSentence CodeSentence
        {
            get { return _codeSentence; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Head == null || value.Head.Functor != _codeSentence.Head.Functor)
                {
                    throw new ArgumentException("Code specified for different procedure.");
                }

                _codeSentence = value;
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
            get { return _prologInstructionStream ?? (_prologInstructionStream = new PrologInstructionStream(this)); }
        }

        public override string ToString()
        {
            return string.Format("{0}.", CodeSentence);
        }

        WamInstructionStream IPrologInstructionStreamContainer.WamInstructionStream
        {
            get { return WamInstructionStream; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal WamInstructionStream WamInstructionStream
        {
            get
            {
                if (_wamInstructionStream == null)
                {
                    var compiler = new Compiler();
                    _wamInstructionStream = compiler.Compile(
                        CodeSentence, 
                        Container.Procedure.Functor, 
                        Index, 
                        IsLast, 
                        Container.Procedure.Container.Program.Libraries,
                        Container.Procedure.Container.Program.IsOptimized);
                }
                return _wamInstructionStream;
            }
        }

        internal void InvalidatePosition()
        {
            RaisePropertyChanged(new PropertyChangedEventArgs("IsFirst"));
            RaisePropertyChanged(new PropertyChangedEventArgs("IsLast"));
        }

        internal void InvalidateInstructionStream()
        {
            _wamInstructionStream = null;

            if (_prologInstructionStream != null)
            {
                _prologInstructionStream = null;
                RaisePropertyChanged(new PropertyChangedEventArgs("PrologInstructionStream"));
            }
        }

        void RaisePropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
