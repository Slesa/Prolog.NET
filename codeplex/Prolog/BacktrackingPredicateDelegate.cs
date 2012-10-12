/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

using Prolog.Code;

namespace Prolog
{
    internal delegate IEnumerable<bool> BacktrackingPredicateDelegate(WamMachine machine, WamReferenceTarget[] arguments);
}
