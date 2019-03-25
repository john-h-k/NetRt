namespace NetRt.Assemblies.PeHeader
{
    public partial struct SectionHeader
    {
        public enum PointerToRelocationsValue : uint
        {
            Default = 0
        }

        public enum PointerToLinenumbersValue : uint
        {
            Default = 0
        }

        public enum NumberOfRelocationsValue : ushort
        {
            Default = 0
        }

        public enum NumberOfLinenumbersValue : ushort
        {
            Default = 0
        }
    }
}
