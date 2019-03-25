namespace NetRt.Assemblies.PeHeader
{
    public partial struct HeaderStandardFields
    {
        public MagicValue Magic;
        public LMajorValue LMajor;
        public LMinorValue LMinor;
        public uint CodeSize;
        public uint InitializedDataSize;
        public uint UninitializedDataSize;
        public uint EntryPointRva;
        public uint BaseOfCode;
        public uint BaseOfData;
    }
}