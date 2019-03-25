using System;
using System.Collections.Generic;
using System.Text;

namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public partial struct HeaderStandardFields
    {
        public enum MagicValue : ushort
        {
            Default = 0x10b
        }

        public enum LMajorValue : byte
        {
            Default = 6
        }

        public enum LMinorValue : byte
        {
            Default = 0
        }
    }
}
