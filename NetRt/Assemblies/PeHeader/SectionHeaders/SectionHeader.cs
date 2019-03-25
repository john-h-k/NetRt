namespace NetRt.Assemblies.PeHeader
{
    public partial struct SectionHeader
    {
        public unsafe fixed char Name[8];
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        
    }
}