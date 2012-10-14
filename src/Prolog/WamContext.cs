/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System.Collections.Generic;

namespace Prolog
{
    internal class WamContext
    {
        public WamContext()
        {
            ArgumentRegisters = new WamReferenceTargetList();
            TemporaryRegisters = new WamReferenceTargetList();
        }

        public WamMachineStates State { get; set; }
        public WamInstructionPointer InstructionPointer { get; set; }
        public IEnumerator<bool> PredicateEnumerator { get; set; }
        public WamInstructionPointer ReturnInstructionPointer { get; set; }
        public int StackIndex { get; set; }
        public WamReferenceTargetList ArgumentRegisters { get; private set; }
        public WamReferenceTargetList TemporaryRegisters { get; private set; }
        public WamChoicePoint ChoicePoint { get; set; }
        public WamEnvironment Environment { get; set; }
        public WamCompoundTerm CurrentStructure { get; set; }
        public WamChoicePoint CutChoicePoint { get; set; }
        public int CurrentStructureIndex { get; set; }
        public UnifyModes CurrentUnifyMode { get; set; }
    }
}
