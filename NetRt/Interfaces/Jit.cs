using NetInterface;
using NetRt.Assemblies;

namespace NetRt.Interfaces
{
    public abstract class Jit
    {
        public static Jit Instance { get; set; }

        public abstract unsafe byte* JitMethod(MethodDef method, byte* il);
    }
}
