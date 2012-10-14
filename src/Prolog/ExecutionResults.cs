/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    /// <summary>
    /// Identifies the success or failure of a Prolog machine instruction.
    /// </summary>
    public enum ExecutionResults
    {
        /// <summary>
        /// Indicates the instruction completed successfully but no results are yet available.
        /// </summary>
        None,

        /// <summary>
        /// Indicates backtracking is required before proceeding.
        /// </summary>
        Backtrack,

        /// <summary>
        /// Indicates the query completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// Indicates the query did not complete successfully.
        /// </summary>
        Failure
    }
}
