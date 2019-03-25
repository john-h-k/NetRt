using System;
using System.Collections.Generic;
using System.Text;

namespace NetRt.Assemblies.PeHeader
{
    public partial struct FileHeader
    {
        public enum MachineValue : ushort
        {
            Default = 0x14c
        }

        public enum PointerToSymbolTableValue : uint
        {
            Default = 0
        }

        public enum NumberOfSymbolTableValue : uint
        {
            Default = 0
        }
    }
}
