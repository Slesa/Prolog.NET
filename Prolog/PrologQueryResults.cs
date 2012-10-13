/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    /// <summary>
    /// Represents the query results returned by a <see cref="PrologMachine"/>.
    /// </summary>
    public sealed class PrologQueryResults : IPrologVariableListContainer
    {
        internal PrologQueryResults()
        {
            Variables = new PrologVariableList(this);
        }

        public PrologVariableList Variables { get; private set; }
    }
}
