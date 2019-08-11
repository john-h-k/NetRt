namespace System
{
    // Intrinsic replaced by the JIT
    // Fields cannot be byref in IL, so this is the workaround
    // Used by Span<T>
    // public but dangerous
    public unsafe ref struct ByReference<T>
    {
#pragma warning disable 169, IDE0051
        private readonly void* _value; // used by JIT
#pragma warning restore 169, IDE0051
        public ByReference(ref T value)
        {
            throw new PlatformNotSupportedException();
        }
        public ref T Value => throw new PlatformNotSupportedException();
    }
}