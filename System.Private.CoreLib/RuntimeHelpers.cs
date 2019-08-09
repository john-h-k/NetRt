using System.Runtime.CompilerServices;

namespace System
{
    public static class RuntimeHelpers
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int GetHashCode(object obj);

        public static bool IsBitwiseEquatable<T>()
        {
            //if (
            //    typeof(T) == typeof(bool)

            //    || typeof(T) == typeof(sbyte)
            //    || typeof(T) == typeof(byte)


            //    || typeof(T) == typeof(short)
            //    || typeof(T) == typeof(ushort)


            //    || typeof(T) == typeof(int)
            //    || typeof(T) == typeof(uint)


            //    || typeof(T) == typeof(long)
            //    || typeof(T) == typeof(ulong)


            //    || typeof(T) == typeof(float)
            //    || typeof(T) == typeof(double)
            //    )
            //{
            //    return true;
            //}

            // TODO If T has BitwiseEquatableAttribute

            return false;
        }

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