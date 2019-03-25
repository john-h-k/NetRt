namespace NetRt.Assemblies.PeHeader
{
    public partial struct WindowsNtFields
    {
        public enum DllFlagsValues : ushort
        {
            Default = 0,
            Mask = unchecked((ushort)~0x100f) // the bits represented by 0x100f must be zero
        }
    }
}