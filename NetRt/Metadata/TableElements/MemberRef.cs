namespace NetRt.Metadata.TableElements
{
    public readonly struct MemberRef
    {
        public MemberRef(uint @class, string name, uint signature)
        {
            Class = @class;
            Name = name;
            Signature = signature;
        }

        public uint Class { get; }
        public string Name { get; }
        public uint Signature { get; }
    }
}