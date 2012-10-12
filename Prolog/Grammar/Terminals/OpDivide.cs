/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Matches slash unless followed immediately by another slash.
    //
    [Terminal(@"/(?!/)")]
    internal sealed class OpDivide : PrologTerminal
    { }
}
