using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetRt.Common
{
    public static unsafe class StreamExtensions
    {
        public static T Read<T>(this Stream stream) where T : unmanaged
        {
            Span<byte> buff = sizeof(T) <= 1024 ? stackalloc byte[sizeof(T)] : new byte[sizeof(T)];
            stream.Read(buff);
            return Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(buff));
        }

        public static T DangerousRead<T>(this Stream stream)
        {
            Span<byte> buff = Unsafe.SizeOf<T>() <= 1024 ? stackalloc byte[Unsafe.SizeOf<T>()] : new byte[Unsafe.SizeOf<T>()];
            stream.Read(buff);
            return Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(buff));
        }

        public static void Skip(this Stream stream, int byteCount)
        {
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));

            stream.Seek(byteCount, SeekOrigin.Current);
        }

        public static void Goto(this Stream stream, long index)
        {
            stream.Seek(index, SeekOrigin.Begin);
        }
    }
}