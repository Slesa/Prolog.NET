/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    /// <summary>
    /// Represents an ordered sequence of <see cref="PrologInstruction"/> objects.
    /// </summary>
    public sealed class PrologInstructionStream : ReadableList<PrologInstruction>
    {
        #region Fields

        IPrologInstructionStreamContainer m_container;
        WamInstructionStream m_wamInstructionStream;

        #endregion

        #region Constructors

        internal PrologInstructionStream(IPrologInstructionStreamContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            m_container = container;

            m_wamInstructionStream = m_container.WamInstructionStream;
            for (int idx = 0; idx < m_wamInstructionStream.Length; ++idx)
            {
                Items.Add(new PrologInstruction(this, idx));
            }
        }

        #endregion

        #region Internal Properties

        internal IPrologInstructionStreamContainer Container
        {
            get { return m_container; }
        }

        internal WamInstructionStream WamInstructionStream
        {
            get { return m_wamInstructionStream; }
        }

        #endregion
    }
}
