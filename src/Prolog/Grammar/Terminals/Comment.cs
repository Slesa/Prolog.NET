/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using Lingua;

namespace Prolog.Grammar
{
    // Matches comments of the form /* comment */.  Note the use of the
    // *? quanitifer that indicates the minimum number of characters should
    // be matched.  This prevents matching separate comments as a single comment
    // with all intervening text as the comment body.
    //
    [Terminal(@"/\*[.\n]*?\*/", Ignore = true)]
    internal sealed class Comment : PrologTerminal
    { }
}
