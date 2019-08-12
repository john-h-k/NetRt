using System.Diagnostics.CodeAnalysis;

namespace NetRt.Metadata.MethodData
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum MethodHeaderFlags : byte
    {
        CorILMethod_FatFormat = 0x3,
        CorILMethod_TinyFormat = 0x2,
        CorILMethod_MoreSects = 0x8,
        CorILMethod_InitLocals = 0x10
    }
}