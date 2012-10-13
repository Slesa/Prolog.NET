/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    /// <summary>
    /// Contains state information passed to an <see cref="EventHandler"/> registered with a <see cref="PrologMachine"/>
    /// upon completion of a query.
    /// </summary>
    public sealed class PrologQueryEventArgs : EventArgs
    {
        internal PrologQueryEventArgs(PrologQueryResults results)
        {
            Results = results;
        }

        static PrologQueryEventArgs()
        {
            Empty = new PrologQueryEventArgs(null);
        }

        public new static PrologQueryEventArgs Empty { get; private set; }
        public PrologQueryResults Results { get; private set; }
    }
}
