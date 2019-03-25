using System.Runtime.CompilerServices;

namespace System
{
    public static class RuntimeHelpers
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(object obj);
    }
}