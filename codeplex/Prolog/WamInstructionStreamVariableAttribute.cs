/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

using System;

namespace Prolog
{
    internal sealed class WamInstructionStreamVariableAttribute : WamInstructionStreamAttribute, IImmuttable
    {
        #region Fields

        private string m_name;
        private WamInstructionRegister m_register;

        #endregion

        #region Constructors

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

            m_name = name;
            m_register = register;
        }

        #endregion

        #region Fields

        public string Name
        {
            get { return m_name; }
        }

        public WamInstructionRegister Register
        {
            get { return m_register; }
        }

        #endregion
    }
}
