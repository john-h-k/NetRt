using System.Diagnostics.CodeAnalysis;

namespace NetRt.Assemblies.Image
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Characteristics : ushort
    {
        IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
        IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
        IMAGE_FILE_32BIT_MACHINE = 0x0100,
        IMAGE_FILE_DLL = 0x2000
    }
}