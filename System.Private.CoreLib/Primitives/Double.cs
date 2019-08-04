using System.Runtime.CompilerServices;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace System
{
    public readonly struct Double
    {
        public const double NaN = 0d / 0d;
        public const double MinValue = -1.7976931348623157E+308;
        public const double MaxValue = 1.7976931348623157E+308;
        public const double PositiveInfinity = MaxValue + 1;
        public const double NegativeInfinity = MinValue - 1;
        public const double Epsilon = 4.9406564584124654E-324;

        private readonly double _value;

        public override int GetHashCode()
        {
            return Unsafe.As<double, long>(ref Unsafe.AsRef(in this)).GetHashCode();
        }

        public override bool Equals(object obj)
            => obj is double d && Equals(d);

        public bool Equals(double d)
            => Equals(this, d);

        public static bool Equals(double left, double right)
        {
            if (left == right) return true;

            return IsNaN(left) && IsNaN(right);
        }

        public static bool IsNaN(double d)
        {
            ulong bits = Unsafe.As<double, ulong>(ref d);
            return (bits & 0x7FFFFFFFFFFFFFFF) > 0x7FF0000000000000;
        }
    }
}