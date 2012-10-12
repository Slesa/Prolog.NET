/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Recognizes periods except when followed by an open parenthesis (i.e. a list functor).
    //
    [Terminal(@"\.(?!\()")]
    internal sealed class Period : PrologTerminal
    { }
}
