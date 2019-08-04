using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PAL
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static unsafe class Crt
    {
        public static IntPtr CrtHandle { get; }

        public static void TryMarshal<TDelegate>(out TDelegate del) where TDelegate : Delegate
            => del = Marshal.GetDelegateForFunctionPointer<TDelegate>(NativeLibrary.GetExport(CrtHandle, typeof(TDelegate).Name.Substring(1)));

        public static void TryMarshal<TDelegate>(out TDelegate del, string name) where TDelegate : Delegate
            => del = Marshal.GetDelegateForFunctionPointer<TDelegate>(NativeLibrary.GetExport(CrtHandle, name));

        static Crt()
        {
            string name = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                name = "msvcrt";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                name = "libc";
            }

            CrtHandle = NativeLibrary.Load(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* malloc(IntPtr size) => CrtMemNativeMethods.malloc(size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* malloc(int size) => CrtMemNativeMethods.malloc((IntPtr)size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* realloc(void* ptr, IntPtr size) => CrtMemNativeMethods.realloc(ptr, size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* realloc(void* ptr, int size) => CrtMemNativeMethods.realloc(ptr, (IntPtr)size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* calloc(IntPtr nmemb, IntPtr size) => CrtMemNativeMethods.calloc(nmemb, size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* calloc(int nmemb, int size) => CrtMemNativeMethods.calloc((IntPtr)nmemb, (IntPtr)size);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void free(void* ptr) => CrtMemNativeMethods.free(ptr);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memcpy(void* dest, [In] void* src, IntPtr count) => CrtMemNativeMethods.memcpy(dest, src, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memcpy(void* dest, [In] void* src, int count) => CrtMemNativeMethods.memcpy(dest, src, (IntPtr)count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memmove(void* dest, [In] void* src, IntPtr count) => CrtMemNativeMethods.memmove(dest, src, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memmove(void* dest, [In] void* src, int count) => CrtMemNativeMethods.memmove(dest, src, (IntPtr)count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memset(void* dest, int n, IntPtr count) => CrtMemNativeMethods.memset(dest, n, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* memset(void* dest, int n, int count) => CrtMemNativeMethods.memset(dest, n, (IntPtr)count);
    }
}