namespace NetRt.Assemblies.PeHeader
{
    public struct OptionalHeader
    {
        public HeaderStandardFields StandardFields;
        public WindowsNtFields NtSpecificFields;
        public HeaderDataDirectories DataDirectories;
    }
}