/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    [Terminal(@"-?[0-9]{1,10}")]
    internal sealed class LiteralInteger : PrologTerminal
    { }
}
