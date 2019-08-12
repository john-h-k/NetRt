using NetRt.Metadata;
using NetRt.Metadata.MethodData;
using NetRt.TypeLoad.TypeSystem;

namespace NetRt.Interfaces
{
    public abstract unsafe class Runtime
    {
        public abstract byte* AllocateMethodMemory(int size);

        public abstract GarbageCollector Gc { get; protected set; }
        public abstract Jit Jit { get; protected set; }

        public abstract MethodInformation GetMethod(int token);
        public abstract TypeInformation GetType(int token);
    }
}
