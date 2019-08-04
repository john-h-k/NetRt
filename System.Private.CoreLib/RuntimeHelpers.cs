using System.Runtime.CompilerServices;

namespace System
{
    public static class RuntimeHelpers
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(object obj);


        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class TypeHandlePun
        {
            public IntPtr DontTouch;
        }
        public static IntPtr GetRawTypeHandle(object obj)
        {
            var pun = Unsafe.As<TypeHandlePun>(obj);
            return Unsafe.Add(ref pun.DontTouch, elementOffset: -1);
        }
    }
}