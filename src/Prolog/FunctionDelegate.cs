/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Prolog.Code;

namespace Prolog
{
    /// <summary>
    /// Represents the method that implements the behavior associated with a <see cref="Function"/>.
    /// </summary>
    /// <param name="arguments">The arguments to be passed to the <see cref="Function"/>.</param>
    /// <returns><value>true</value> if the <see cref="Function"/> completes successfully.  Otherwise, <value>false</value>.</returns>
    public delegate CodeTerm FunctionDelegate(CodeTerm[] arguments);
}
