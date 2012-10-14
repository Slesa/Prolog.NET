/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    internal sealed class WamChoicePoint
    {
        public static int NextGeneration;

        public WamChoicePoint(WamChoicePoint predecessor, WamEnvironment environment, int stackIndex, WamInstructionPointer returnInstructionPointer, IEnumerable<WamReferenceTarget> argumentRegisters, WamChoicePoint cutChoicePoint)
        {
            Generation = NextGeneration++;

            Predecessor = predecessor;
            Environment = environment;
            StackIndex = stackIndex;
            ReturnInstructionPointer = returnInstructionPointer;
            ArgumentRegisters = new WamReferenceTargetList(argumentRegisters);
            CutChoicePoint = cutChoicePoint;

            BacktrackInstructionPointer = WamInstructionPointer.Undefined;
            PredicateEnumerator = null;

            Trail = new List<WamVariable>();
        }

        public int Generation { get; private set; }
        public WamChoicePoint Predecessor { get; private set; }
        public WamEnvironment Environment { get; private set; }
        public int StackIndex { get; private set; }
        public WamInstructionPointer ReturnInstructionPointer { get; private set; }
        public WamReferenceTargetList ArgumentRegisters { get; private set; }
        public WamChoicePoint CutChoicePoint { get; private set; }
        public WamInstructionPointer BacktrackInstructionPointer { get; internal set; }
        public IEnumerator<bool> PredicateEnumerator { get; internal set; }
        public List<WamVariable> Trail { get; private set; }
    }
}
