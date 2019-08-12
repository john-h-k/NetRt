using System.Diagnostics.CodeAnalysis;

namespace NetRt.Metadata.MethodData
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum EhKind : ushort
    {
        COR_ILEXCEPTION_CLAUSE_EXCEPTION = 0x0000,
        COR_ILEXCEPTION_CLAUSE_FILTER = 0x0001,
        COR_ILEXCEPTION_CLAUSE_FINALLY = 0x0002,
        COR_ILEXCEPTION_CLAUSE_FAULT = 0x0004
    }
}