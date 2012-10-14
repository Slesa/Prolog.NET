/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Recognizes:
    // o strings containing letters, numbers and underscores beginning with a lower-case letter.
    // o periods when followed immediately by an open parenthesis (i.e. a list functor).
    //
    [Terminal(@"[\p{Ll}][\p{L}\p{N}_]{0,99}|\.(?=\()")]
    internal sealed class Atom : PrologTerminal
    { }
}
