using System.Runtime.InteropServices;

namespace NetRt.Assemblies
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CliHeader
    {
        public uint Cb;
        public ushort MajorRuntimeVersion;
        public ushort MinorRuntimeVersion;
        public ulong MetaData;
        public RuntimeFlags Flags;
        public uint EntryPointToken;
        public ulong Resources;
        public ulong StrongNameSignature;
        public ulong CodeManagerTable;
        public ulong VTableFixups;
        public ulong ExportAddressTableJumps;
        public ulong ManagedNativeHeader;

        //public fixed byte Header[72];
    }
}