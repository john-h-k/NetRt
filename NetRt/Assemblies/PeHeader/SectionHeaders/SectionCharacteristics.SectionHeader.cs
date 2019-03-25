// ReSharper disable InconsistentNaming
namespace NetRt.Assemblies.PeHeader
{
    public partial struct SectionHeader
    {
        public enum Characteristics : uint
        {
            IMAGE_SCN_CNT_CODE = 0x00000020, // Section contains code.
            IMAGE_SCN_CNT_INITIALIZED_DATA = 0x00000040, // Section contains initialized data.
            IMAGE_SCN_CNT_UNINITIALIZED_DATA = 0x00000080, // Section contains uninitialized data.
            IMAGE_SCN_MEM_EXECUTE = 0x20000000, // Section can be executed as code.
            IMAGE_SCN_MEM_READ = 0x40000000, // Section can be read.
            IMAGE_SCN_MEM_WRITE = 0x80000000, // Section can be written to.
        }
    }
}
