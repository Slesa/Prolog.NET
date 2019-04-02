/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@"\\=="/*, Priority = 2*/)] // see OpCannotUnify
    internal sealed class OpTermUnequal : PrologTerminal
    { }
}
