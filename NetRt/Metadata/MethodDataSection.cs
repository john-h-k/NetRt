using System.Collections.Immutable;

namespace NetRt.Metadata
{
    public readonly struct MethodDataSection
    {
        public MethodDataSection(SectionKind kind, uint dataSize, ImmutableArray<ExceptionHandlingClause> exceptionHandlingClauses)
        {
            Kind = kind;
            DataSize = dataSize;
            ExceptionHandlingClauses = exceptionHandlingClauses;
        }

        public bool IsThin => !Kind.HasFlag(SectionKind.CorILMethod_Sect_FatFormat);
        public bool IsFinalSection => !Kind.HasFlag(SectionKind.CorILMethod_Sect_MoreSects);

        public SectionKind Kind { get; }
        public uint DataSize { get; }
        public ImmutableArray<ExceptionHandlingClause> ExceptionHandlingClauses { get; }
    }
}