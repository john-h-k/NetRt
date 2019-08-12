using NetRt.TypeLoad.TypeSystem;

namespace NetRt.Metadata.MethodData
{
    public struct RetType
    {
        public RetType(TypeInformation type, CustomMod? modifier = null)
        {
            Modifier = modifier;
            Type = type;
        }

        public CustomMod? Modifier { get; }
        public TypeInformation Type { get; }
    }
}
