/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@":=")] 
    internal sealed class OpIs1 : PrologTerminal
    { }

    [Terminal(@"is(?![\p{L}\p{N}_])", Priority = 1)] // See Atom
    internal sealed class OpIs2 : PrologTerminal
    { }
}
