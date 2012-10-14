/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Matches comments of the form // comment.
    //
    [Terminal(@"//(?!/).*(?=\n)", Ignore = true)]
    internal sealed class LineComment : PrologTerminal
    { }
}
