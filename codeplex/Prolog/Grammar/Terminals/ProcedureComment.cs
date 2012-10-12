/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Matches comments of the form /// comment.  Procedure comments are not
    // treated as white-space.  They are recognized by the grammer and may
    // only preceed procedure clause definitions.
    //
    [Terminal(@"///.*(?=\n)")]
    internal sealed class ProcedureComment : PrologTerminal
    { }
}
