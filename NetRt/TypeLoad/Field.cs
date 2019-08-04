using System.Reflection;

namespace NetRt.TypeLoad
{
    public readonly struct Field
    {
        public Field(FieldAttributes flags, string name, uint signature)
        {
            Flags = flags;
            Name = name;
            Signature = signature;
        }

        public FieldAttributes Flags { get; }
        public string Name { get; }
        public uint Signature { get; }
    }
}