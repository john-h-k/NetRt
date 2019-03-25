namespace NetRt.Assemblies.PeHeader
{
    public partial struct FileHeader
    {
        public MachineValue Machine; // Must be 0x14c (MachineValue.Default)
        public ushort NumberOfSections;
        public UnixSecondsTime DateStamp;
        public PointerToSymbolTableValue PointerToSymbolTable; // Always 0
        public NumberOfSymbolTableValue NumberOfSymbols; // Always 0
        public ushort OptionalHeaderSize;
        public ImageCharacteristics Characteristics;
    }
}