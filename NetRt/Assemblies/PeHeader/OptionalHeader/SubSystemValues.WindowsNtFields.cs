// ReSharper disable InconsistentNaming
namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public partial struct WindowsNtFields
    {
        public enum SubSystemValues : ushort
        {
            IMAGE_SUBSYSTEM_WINDOWS_CUI = 0x3,
            IMAGE_SUBSYSTEM_WINDOWS_GUI = 0x2
        }
    }
}