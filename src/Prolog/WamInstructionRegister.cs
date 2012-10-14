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
        static readonly WamInstructionRegister _unused = new WamInstructionRegister(WamInstructionRegisterTypes.Unused, 0);

        WamInstructionRegisterTypes _type; // 1 byte
        byte _id;

        public WamInstructionRegister(WamInstructionRegisterTypes type, byte id)
        {
            _type = type;
            _id = id;
        }

        public static WamInstructionRegister Unused
        {
            get { return _unused; }
        }

        public WamInstructionRegisterTypes Type
        {
            get { return _type; }
        }

        public byte Id
        {
            get { return _id; }
        }

        public bool IsUnused
        {
            get { return _type == WamInstructionRegisterTypes.Unused; }
        }

        public bool IsUsed
        {
            get { return _type != WamInstructionRegisterTypes.Unused; }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", TypePrefix, Id);
        }

        string TypePrefix
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
    }
}
