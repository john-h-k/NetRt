using System.Dynamic;

namespace NetRt.Metadata.MethodData
{
    public enum CustomModType : byte
    {
        Required = 0x1F,
        Optional = 0x20
    }
    public readonly struct CustomMod
    {
        public CustomMod(CustomModType customModType, uint typeDefOrRefOrSpec)
        {
            CustomModType = customModType;
            TypeDefOrRefOrSpec = typeDefOrRefOrSpec;
        }

        public CustomModType CustomModType { get; }
        public uint TypeDefOrRefOrSpec { get; }
    }
}