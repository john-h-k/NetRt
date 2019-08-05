using System.Diagnostics.CodeAnalysis;

namespace NetRt.Metadata
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum SectionKind : byte
    {
        CorILMethod_Sect_EHTable = 0x1,
        CorILMethod_Sect_OptILTable = 0x2, // Unused
        CorILMethod_Sect_FatFormat = 0x40,
        CorILMethod_Sect_MoreSects = 0x80,
    }
}