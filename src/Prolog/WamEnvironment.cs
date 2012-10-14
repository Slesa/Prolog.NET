/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal sealed class WamEnvironment
    {
        static int _nextId;

        public WamEnvironment(WamEnvironment predecessor, WamInstructionPointer returnInstructionPointer, WamChoicePoint cutChoicePoint)
        {
            Id = _nextId++;
            Predecessor = predecessor;
            ReturnInstructionPointer = returnInstructionPointer;
            CutChoicePoint = cutChoicePoint;

            PermanentRegisters = new WamReferenceTargetList();
        }

        public int Id { get; private set; }
        public WamEnvironment Predecessor { get; private set; }
        public WamInstructionPointer ReturnInstructionPointer { get; private set; }
        public WamReferenceTargetList PermanentRegisters { get; private set; }
        public WamChoicePoint CutChoicePoint { get; private set; }
    }
}
