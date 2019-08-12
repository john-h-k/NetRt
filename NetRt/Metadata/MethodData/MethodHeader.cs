namespace NetRt.Metadata
{
    public readonly struct MethodHeader
    {
        public MethodHeader(MethodHeaderFlags flags, byte headerSize, ushort maxStack, uint codeSize, uint localVarSigToken)
        {
            Flags = flags;
            HeaderSize = headerSize;
            MaxStack = maxStack;
            CodeSize = codeSize;
            LocalVarSigToken = localVarSigToken;
        }

        public MethodHeader(MethodHeaderFlags flags, uint codeSize)
        {
            Flags = flags;
            HeaderSize = 1;
            MaxStack = 8;
            CodeSize = codeSize;
            LocalVarSigToken = NoLocals;
        }

        public const int NoLocals = 0;

        public bool IsTiny => Flags == MethodHeaderFlags.CorILMethod_TinyFormat;

        public MethodHeaderFlags Flags { get; }
        public uint HeaderSize { get; }
        public ushort MaxStack { get; }
        public uint CodeSize { get; }
        public uint LocalVarSigToken { get; }

    }
}