using System.Reflection.PortableExecutable;

namespace NetRt.Assemblies.PeHeader
{
    public partial struct SectionHeader
    {
        public unsafe fixed char Name[8];
        public uint VirtualSize;
        public uint VirtualAddress;
        public uint SizeOfRawData;
        public uint PointerToRawData;
        public PointerToRelocationsValue PointerToRelocations;
        public PointerToLinenumbersValue PointerToLinenumbers;
        public NumberOfRelocationsValue NumberOfRelocations;
        public NumberOfLinenumbersValue NumberOfLinenumbers;
        public SectionCharacteristics Characteristics;
    }
}