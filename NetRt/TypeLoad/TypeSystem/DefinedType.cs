using NetInterface;

namespace NetRt.TypeLoad.TypeSystem
{
    public class DefinedType : TypeDefinition
    {
        public override Field[] Fields { get; }
        public override MethodDef[] Methods { get; }

        public override bool IsObject => true;
    }
}