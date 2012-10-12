/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@"mod(?![a-zA-Z0-9_])", Priority = -1)]
    internal sealed class OpModulo : PrologTerminal
    { }
}
