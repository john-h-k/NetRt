using System;

namespace NetInterface
{
    public abstract class Jit
    {
        public static Jit Instance { get; set; }

        public abstract unsafe byte* JitMethod(MethodDef method);
    }
}
