using NetInterface;
using NetRt.Assemblies;

namespace NetRt.TypeLoad.TypeSystem
{
    public class DefinedType : TypeInformation
    {
        public override int Size { get; }
        public override Field[] Fields { get; }
        public override MethodDef[] Methods { get; }

        public override bool IsObject => true;
    }
}