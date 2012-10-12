/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    internal abstract class WamInstructionStreamAttribute : IImmuttable
    {
        #region Fields

        private int m_index;

        #endregion

        #region Constructors

        public WamInstructionStreamAttribute(int index)
        {
            m_index = index;
        }

        #endregion

        #region Public Properties

        public int Index
        {
            get { return m_index; }
        }

        #endregion
    }
}
