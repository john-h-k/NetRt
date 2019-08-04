using System;
using System.Diagnostics.CodeAnalysis;
// ReSharper disable BuiltInTypeReferenceStyle

namespace NetRt.Assemblies
{
    using Rva = UInt32;

    public sealed class Section
    {
        public string Name { get; set; }
        public uint VirtualSize { get; set; }
        public Rva VirtualAddress { get; set; }
        public uint SizeOfRawData { get; set; }
        public uint PointerToRawData { get; set; }
        public SectionCharacteristics Characteristics { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum SectionCharacteristics : uint
        {
            IMAGE_SCN_CNT_CODE = 0x00000020,
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040,
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080,
            IMAGE_SCN_MEM_EXECUTE = 0x20000000,
            IMAGE_SCN_MEM_READ = 0x40000000,
            IMAGE_SCN_MEM_WRITE = 0x80000000
        }
    }
}