namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public partial struct HeaderDataDirectories
    {
        public ExportTableValue ExportTable;
        public ulong ImportTable;
        public ResourceTableValue ResourceTable;
        public ExceptionTableValue ExceptionTable;
        public CertificateTableValue CertificateTable;
        public ulong BaseRelocationTable;
        public DebugValue Debug;
        public CopyrightValue Copyright;
        public GlobalPtrValue GlobalPtr;
        public TlsTableValue TlsTable;
        public LoadConfigTableValue LoadConfigTable;
        public BoundImportValue BoundImport;
        public ulong Iat;
        public DelayImportDescriptorValue DelayImportDescriptor;
        public ulong CliHeader;
        public ReservedValue Reserved;
    }
}