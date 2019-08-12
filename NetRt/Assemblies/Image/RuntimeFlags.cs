using System.Diagnostics.CodeAnalysis;

namespace NetRt.Assemblies.Image
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum RuntimeFlags : uint
    {
        COMIMAGE_FLAGS_ILONLY = 0x00000001,
        COMIMAGE_FLAGS_32BITREQUIRED = 0x00000002,
        COMIMAGE_FLAGS_STRONGNAMESIGNED = 0x00000008,
        COMIMAGE_FLAGS_NATIVE_ENTRYPOINT = 0x00000010 
    }
}