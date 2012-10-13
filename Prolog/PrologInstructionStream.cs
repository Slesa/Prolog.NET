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
        internal PrologInstructionStream(IPrologInstructionStreamContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            Container = container;

            WamInstructionStream = Container.WamInstructionStream;
            for (int idx = 0; idx < WamInstructionStream.Length; ++idx)
            {
                Items.Add(new PrologInstruction(this, idx));
            }
        }

        internal IPrologInstructionStreamContainer Container { get; private set; }
        internal WamInstructionStream WamInstructionStream { get; private set; }
    }
}
