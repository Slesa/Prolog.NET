/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal enum WamInstructionRegisterTypes : byte
    {
        Unused = 0,
        Argument,
        Temporary,
        Permanent
    }
}
