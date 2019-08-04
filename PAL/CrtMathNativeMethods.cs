using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace PAL
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CrtMathNativeMethods
    {
        public static readonly _doubleMath cos;
        public static readonly _doubleMath sin;
        public static readonly _doubleMath tan;
        public static readonly _doubleMath acos;
        public static readonly _doubleMath asin;
        public static readonly _doubleMath atan;
        public static readonly _doubleMath cosh;
        public static readonly _doubleMath sinh;
        public static readonly _doubleMath tanh;
        public static readonly _doubleMath acosh;
        public static readonly _doubleMath asinh;
        public static readonly _doubleMath atanh;
        public static readonly _atan2 atan2;

        static CrtMathNativeMethods()
        {
            Crt.TryMarshal(out cos, nameof(cos));
            Crt.TryMarshal(out sin, nameof(sin));
            Crt.TryMarshal(out tan, nameof(tan));
            Crt.TryMarshal(out acos, nameof(acos));
            Crt.TryMarshal(out asin, nameof(asin));
            Crt.TryMarshal(out atan, nameof(atan));
            Crt.TryMarshal(out cosh, nameof(cosh));
            Crt.TryMarshal(out sinh, nameof(sinh));
            Crt.TryMarshal(out tanh, nameof(tanh));
            Crt.TryMarshal(out acosh, nameof(acosh));
            Crt.TryMarshal(out asinh, nameof(asinh));
            Crt.TryMarshal(out atanh, nameof(atanh));
            Crt.TryMarshal(out atan2);
        }
        
        // Why use many delegate when few delegate do trick

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate double _doubleMath(double x);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate double _atan2(double x, double y);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Auto,
            SetLastError = false, ThrowOnUnmappableChar = true)]
        public delegate float _singleMath(float x);
    }
}