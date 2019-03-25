namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public partial struct WindowsNtFields
    {
        public uint ImageBase;
        public uint SectionAlignment;
        public FileAlignmentValue FileAlignment;
        public OsMajorValue OsMajor;
        public OsMinorValue OsMinor;
        public UserMajorValue UserMajor;
        public UserMinorValue UserMinor;
        public SubSysMajorValue SubSysMajor;
        public SubSysMinorValue SubSysMinor;
        public ReservedValue Reserved;
        public uint ImageSize;
        public uint HeaderSize;
        public FileChecksumValue FileChecksum;
        public SubSystemValues SubSystem;
        public DllFlagsValues DllFlags;
        public StackReserveSizeValue StackReserveSize;
        public StackCommitSizeValue StackCommitSize;
        public HeapReserveSizeValue HeapReserveSize;
        public HeapCommitSizeValue HeapCommitSize;
        public LoaderFlagsValue LoaderFlags;
        public NumberOfDataDirectoriesValue NumberOfDataDirectories;
    }
}