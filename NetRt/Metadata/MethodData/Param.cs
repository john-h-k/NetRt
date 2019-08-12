using System.Reflection;

namespace NetRt.Metadata.MethodData
{
    public readonly struct Param
    {
        public Param(ParameterAttributes flags, ushort sequence, string name)
        {
            Flags = flags;
            Sequence = sequence;
            Name = name;
        }

        public ParameterAttributes Flags { get; }
        public ushort Sequence { get; }
        public string Name { get; }
    }
}