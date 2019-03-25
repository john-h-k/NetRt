using System;

// ReSharper disable InconsistentNaming, IdentifierTypo

namespace NetRt.Assemblies
{
    [Flags]
    public enum RuntimeFlags : uint
    {
        COMIMAGE_FLAGS_ILONLY = 0x00000001,
        COMIMAGE_FLAGS_32BITREQUIRED = 0x00000002,
        COMIMAGE_FLAGS_STRONGNAMESIGNED = 0x00000008,
        COMIMAGE_FLAGS_NATIVE_ENTRYPOINT = 0x00000010,
        COMIMAGE_FLAGS_TRACKDEBUGDATA = 0x00010000 
    }
}