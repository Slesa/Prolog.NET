/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

using Prolog.Code;

namespace Prolog
{
    /// <summary>
    /// Represents a query to be evaluated by a <see cref="PrologMachine"/>.
    /// </summary>
    public sealed class Query
    {
        #region Fields

        private CodeSentence m_codeSentence;

        private LibraryList m_libraries;

        private WamInstructionStream m_wamInstructionStream;

        #endregion

        #region Constructors

        public Query(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            m_codeSentence = codeSentence;

            m_libraries = LibraryList.Create();
            m_libraries.Add(Library.Standard);
        }

        #endregion

        #region Public Properties

        public CodeSentence CodeSentence
        {
            get { return m_codeSentence; }
        }

        public LibraryList Libraries
        {
            get { return m_libraries; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{0}", CodeSentence);
        }

        #endregion

        #region Internal Members

        internal WamInstructionStream WamInstructionStream
        {
            get
            {
                if (m_wamInstructionStream == null)
                {
                    Compiler compiler = new Compiler();
                    m_wamInstructionStream = compiler.Compile(CodeSentence, Libraries, false);
                }

                return m_wamInstructionStream;
            }
        }

        #endregion
    }
}
