using NetRt.TypeLoad.TypeSystem;

namespace NetRt.Metadata.MethodData
{
    public enum MethodDefSigFlags : byte
    {
        HasThis = 0x20,
        ExplicitThis = 0x40,
        Default = 0x0,
        Vararg = 0x5,
        Generic = 0x10,
    }

    public enum ManagedCallingConv
    {
        Default = 0x0,
        Vararg = 0x5,
        Generic = 0x10
    }

    public struct MethodDefSig
    {
        public MethodDefSig(bool hasThis, bool explicitThis, ManagedCallingConv managedCallingConv, uint genParamCount, uint paramCount, RetType retType, TypeInformation[] @params)
        {
            HasThis = hasThis;
            ExplicitThis = explicitThis;
            ManagedCallingConv = managedCallingConv;
            ParamCount = paramCount;
            RetType = retType;
            Params = @params;
            GenParamCount = genParamCount;
        }

        public bool HasThis { get; }
        public bool ExplicitThis { get; }
        public ManagedCallingConv ManagedCallingConv { get; }
        public uint GenParamCount { get; }
        public uint ParamCount { get; }
        public RetType RetType { get; }
        public TypeInformation[] Params { get; }
    }
}