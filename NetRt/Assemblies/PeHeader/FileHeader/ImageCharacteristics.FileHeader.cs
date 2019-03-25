using System;

// ReSharper disable InconsistentNaming, IdentifierTypo

namespace NetRt.Assemblies.PeHeader.FileHeader
{
    public partial struct FileHeader
    {
        [Flags]
        public enum ImageCharacteristics : ushort
        {
            IMAGE_FILE_RELOCS_STRIPPED = 0x0001,
            IMAGE_FILE_EXECUTABLE_IMAGE = 0x0002,
            IMAGE_FILE_32BIT_MACHINE = 0x0100,
            IMAGE_FILE_DLL = 0x2000
        }
    }
}