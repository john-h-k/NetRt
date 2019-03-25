namespace NetRt.Assemblies.PeHeader
{
    public partial struct SectionHeader
    {
        public enum PointerToRelocations : uint
        {
            Default = 0
        }

        public enum PointerToLinenumbers : uint
        {
            Default = 0
        }

        public enum NumberOfRelocations : ushort
        {
            Default = 0
        }

        public enum NumberOfLinenumbers : ushort
        {
            Default = 0
        }
    }
}
