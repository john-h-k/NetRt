namespace NetRt.Assemblies.PeHeader
{
    public unsafe struct PeHeaders
    {
        private const int LfaNewStartIndex = 0x3c;

        public fixed byte MsDosHeader[128];
        public FileHeader FileHeader;
        public OptionalHeader OptionalHeader;
        public fixed byte SectionHeaders[40];
        public fixed byte Iat[40];

        // TODO - relocations

        public uint LfaNew
        {
            get
            {
                fixed (byte* p = MsDosHeader) return *(uint*)(p + LfaNewStartIndex);
            }
        }
    }
}