namespace NetRt.Metadata.TableElements
{
    public readonly struct TypeSpec
    {
        public TypeSpec(uint signature)
        {
            Signature = signature;
        }

        public uint Signature { get; }
    }
}