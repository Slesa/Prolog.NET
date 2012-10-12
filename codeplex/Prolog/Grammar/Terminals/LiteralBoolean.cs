/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@"(true|false)(?![\p{L}\p{N}_])", Priority = 1)] // See Atom
    internal sealed class LiteralBoolean : PrologTerminal
    { }
}
