using NetInterface;
using NetRt.Assemblies;
using NetRt.Metadata;

namespace NetRt.Interfaces
{
    public abstract class Jit
    {
        public static Jit Instance { get; set; }

        public abstract unsafe byte* JitMethod(MethodInformation method);
    }
}
