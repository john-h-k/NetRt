using System.Runtime.CompilerServices;

namespace System
{
#if BIT64
    using nint = Int64;
#else
    using nint = Int32;
#endif

    public readonly struct IntPtr
    {
        private readonly nint _value;

        [Intrinsic]
        public static readonly IntPtr Zero = default;

        public IntPtr(int value) => _value = value;

        public IntPtr(long value)
        {
            _value = checked((nint)value);
        }

        [Intrinsic]
        public static IntPtr operator +(IntPtr left, IntPtr right)
        {
            return new IntPtr(left._value + right._value);
        }

        [Intrinsic]
        public static IntPtr operator +(IntPtr left, int right)
        {
            return new IntPtr(left._value + right);
        }

        [Intrinsic]
        public static IntPtr operator +(int left, IntPtr right)
        {
            return new IntPtr(left + right._value);
        }

        [Intrinsic]
        public static IntPtr operator -(IntPtr left, IntPtr right)
        {
            return new IntPtr(left._value - right._value);
        }

        [Intrinsic]
        public static IntPtr operator -(IntPtr left, int right)
        {
            return new IntPtr(left._value - right);
        }

        [Intrinsic]
        public static IntPtr operator -(int left, IntPtr right)
        {
            return new IntPtr(left - right._value);
        }

        [Intrinsic]
        public static IntPtr operator *(IntPtr left, IntPtr right)
        {
            return new IntPtr(left._value * right._value);
        }

        [Intrinsic]
        public static IntPtr operator *(IntPtr left, int right)
        {
            return new IntPtr(left._value * right);
        }

        [Intrinsic]
        public static IntPtr operator *(int left, IntPtr right)
        {
            return new IntPtr(left * right._value);
        }

        [Intrinsic]
        public static IntPtr operator /(IntPtr left, IntPtr right)
        {
            return new IntPtr(left._value / right._value);
        }

        [Intrinsic]
        public static IntPtr operator /(IntPtr left, int right)
        {
            return new IntPtr(left._value / right);
        }

        [Intrinsic]
        public static IntPtr operator /(int left, IntPtr right)
        {
            return new IntPtr(left / right._value);
        }

        [Intrinsic]
        public static IntPtr operator %(IntPtr left, IntPtr right)
        {
            return new IntPtr(left._value % right._value);
        }

        [Intrinsic]
        public static IntPtr operator %(IntPtr left, int right)
        {
            return new IntPtr(left._value % right);
        }

        [Intrinsic]
        public static IntPtr operator %(int left, IntPtr right)
        {
            return new IntPtr(left % right._value);
        }

        //[Intrinsic]
        //public static explicit operator IntPtr(int value) => new IntPtr(value);
        [Intrinsic]
        public static explicit operator IntPtr(long value) => new IntPtr(value);
        [Intrinsic]
        public static implicit operator IntPtr(int value) => new IntPtr(value);

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}