using NetInterface;
using NetRt.Assemblies;

namespace NetRt.TypeLoad.TypeSystem
{
    public abstract class TypeInformation
    {
        public abstract int Size { get; }
        public abstract Field[] Fields { get; }
        public abstract MethodDef[] Methods { get; }

        public abstract bool IsObject { get; } // false if ByRef/Pointer/not derived from object
    }
}