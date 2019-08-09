using System.Collections.Immutable;
using System.Linq;

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

        public override string ToString() =>
            $"MethodDataSection: \n" +
            $"{(IsThin ? "Thin" : "Fat")} Format\n" +
            $"{(IsFinalSection ? "Final section for this method" : "Has more sections after this section")}\n" +
            $"SectionKind: {Kind}\n" +
            $"DataSize: {DataSize}\n" +
            $"{(ExceptionHandlingClauses.Length > 0 ? $"EhClauses: {string.Join(separator: ' ', ExceptionHandlingClauses.Select(eh => eh.ToString()))}" : string.Empty)}";

        public bool IsThin => !Kind.HasFlag(SectionKind.CorILMethod_Sect_FatFormat);
        public bool IsFinalSection => !Kind.HasFlag(SectionKind.CorILMethod_Sect_MoreSects);
        public SectionKind Kind { get; }
        public uint DataSize { get; }
        public ImmutableArray<ExceptionHandlingClause> ExceptionHandlingClauses { get; }
    }
}