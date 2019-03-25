namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public struct OptionalHeader
    {
        public HeaderStandardFields StandardFields;
        public WindowsNtFields NtSpecificFields;
        public HeaderDataDirectories DataDirectories;
    }
}