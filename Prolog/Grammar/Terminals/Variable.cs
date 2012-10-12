/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@"[\p{Lu}][\p{L}\p{N}_]{0,99}")]
    internal sealed class Variable : PrologTerminal
    { }
}
