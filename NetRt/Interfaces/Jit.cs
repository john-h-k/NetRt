using System;
using NetInterface;
using NetRt.Assemblies;
using NetRt.Metadata;

namespace NetRt.Interfaces
{
    public abstract class Jit
    {
        public static Jit Instance { get; set; }

        public abstract void Initialize(MethodInformation method);

        public abstract unsafe byte* JitMethod();

        public abstract string DumpIl();
        public abstract string CreateJitDump();
        public abstract bool TryDumpIl(Span<char> buffer, out int charsWritten);
    }
}
