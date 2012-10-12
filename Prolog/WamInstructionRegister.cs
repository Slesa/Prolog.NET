/* Copyright © 2010 Richard G. Todd.
 * Licensed under the terms of the Microsoft Public License (Ms-PL).
 */

namespace Prolog
{
    /// <remarks>
    /// Total declared size of member data: 2 bytes
    /// </remarks>
    internal struct WamInstructionRegister : IImmuttable
    {
        #region Fields

        private static WamInstructionRegister s_unused = new WamInstructionRegister(WamInstructionRegisterTypes.Unused, 0);

        private WamInstructionRegisterTypes m_type; // 1 byte
        private byte m_id;

        #endregion

        #region Constructors

        public WamInstructionRegister(WamInstructionRegisterTypes type, byte id)
        {
            m_type = type;
            m_id = id;
        }

        #endregion

        #region Public Properties

        public static WamInstructionRegister Unused
        {
            get { return s_unused; }
        }

        public WamInstructionRegisterTypes Type
        {
            get { return m_type; }
        }

        public byte Id
        {
            get { return m_id; }
        }

        public bool IsUnused
        {
            get { return m_type == WamInstructionRegisterTypes.Unused; }
        }

        public bool IsUsed
        {
            get { return m_type != WamInstructionRegisterTypes.Unused; }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("{0}{1}", TypePrefix, Id);
        }

        #endregion

        #region Hidden Members

        private string TypePrefix
        {
            get
            {
                switch (Type)
                {
                    case WamInstructionRegisterTypes.Argument: return "A";
                    case WamInstructionRegisterTypes.Permanent: return "Y";
                    case WamInstructionRegisterTypes.Temporary: return "X";
                    default: return "?";
                }
            }
        }

        #endregion
    }
}
