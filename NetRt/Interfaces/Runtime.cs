using NetInterface;

namespace NetRt.Interfaces
{
    public abstract unsafe class Runtime
    {
        public abstract byte* AllocateMethodMemory(int size);


        public abstract GarbageCollector Gc { get; protected set; }
        public abstract Jit Jit { get; protected set; }
    }
}
