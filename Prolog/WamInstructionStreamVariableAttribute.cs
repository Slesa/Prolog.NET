/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class WamInstructionStreamVariableAttribute : WamInstructionStreamAttribute, IImmuttable
    {
        public WamInstructionStreamVariableAttribute(int index, string name, WamInstructionRegister register)
            : base(index)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }
            if (register.IsUnused)
            {
                throw new ArgumentException("Register is unused.", "register");
            }
            Name = name;
            Register = register;
        }

        public string Name { get; private set; }
        public WamInstructionRegister Register { get; private set; }
    }
}
