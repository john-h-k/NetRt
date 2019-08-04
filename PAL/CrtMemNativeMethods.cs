using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PAL
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static unsafe class CrtMemNativeMethods
    {
        static CrtMemNativeMethods()
        {
            Crt.TryMarshal(out malloc);
            Crt.TryMarshal(out realloc);
            Crt.TryMarshal(out calloc);
            Crt.TryMarshal(out free);
            Crt.TryMarshal(out memcpy);
            Crt.TryMarshal(out memmove);
            Crt.TryMarshal(out memset);
        }

        public static readonly _malloc malloc;
        public static readonly _realloc realloc;
        public static readonly _calloc calloc;
        public static readonly _free free;
        public static readonly _memcpy memcpy;
        public static readonly _memmove memmove;
        public static readonly _memset memset;

        #region Delegates

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _malloc(IntPtr size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _realloc(void* ptr, IntPtr size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _calloc(IntPtr nmemb, IntPtr size);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void _free(void* ptr);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _memcpy(void* dest, [In] void* src, IntPtr count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _memmove(void* dest, [In] void* src, IntPtr count);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate void* _memset(void* dest, int n, IntPtr count);

        #endregion
    }
}