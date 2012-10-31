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
    public class Query
    {
        private WamInstructionStream _wamInstructionStream;

        public Query(CodeSentence codeSentence)
        {
            if (codeSentence == null)
            {
                throw new ArgumentNullException("codeSentence");
            }

            CodeSentence = codeSentence;

            Libraries = LibraryList.Create();
            Libraries.Add(Library.Standard);
        }

        public CodeSentence CodeSentence { get; private set; }
        public LibraryList Libraries { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}", CodeSentence);
        }

        internal WamInstructionStream WamInstructionStream
        {
            get
            {
                if (_wamInstructionStream == null)
                {
                    var compiler = new Compiler();
                    _wamInstructionStream = compiler.Compile(CodeSentence, Libraries, false);
                }
                return _wamInstructionStream;
            }
        }
    }
}
