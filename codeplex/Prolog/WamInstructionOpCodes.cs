/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal enum WamInstructionOpCodes : byte
    {
        Noop = 0,
        Success,
        Backtrack,
        Failure,

        PutStructure,
        PutUnboundVariable,
        PutBoundVariable,
        PutValue,

        SetUnboundVariable,
        SetBoundVariable,
        SetValue,

        GetStructure,
        GetUnboundVariable,
        GetBoundVariable,
        GetValue,

        UnifyUnboundVariable,
        UnifyBoundVariable,
        UnifyValue,

        Call,
        Execute,
        LibraryCall,
        Proceed,
        Cut,

        Allocate,
        Deallocate,

        TryMeElse,
        RetryMeElse,
        TrustMe
    }
}
