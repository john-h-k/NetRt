using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed unsafe class String : object
    {
        public const string Empty = "";

#pragma warning disable 649
        private int _stringLength;
#pragma warning restore 649
        private char _firstChar;

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public int Length => _stringLength;

        public ref readonly char GetPinnableReference() => ref _firstChar;

        public char this[int index]
        {
            [Intrinsic]
            get
            {
                if ((uint)index > (uint)_stringLength)
                {
                    throw new ArgumentOutOfRangeException("Index was outside the bounds of the array.");
                }

                return Unsafe.Add(ref _firstChar, index);
            }
        }

        public static bool operator ==(string left, string right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            if (left.Length != right.Length) return false;
            //if (left?.Length != right?.Length) return false;

            fixed (char* l = left)
            fixed (char* r = right)
            {
                return memcmp(l, r, (IntPtr)left.Length) == 0;
            }
        }

        public static bool operator !=(string left, string right)
        {
            // ReSharper disable once NegativeEqualityExpression
            return !(left == right);
        }

        [DllImport("libc")]
        private static extern int memcmp(char* left, char* right, IntPtr count);
    }
}