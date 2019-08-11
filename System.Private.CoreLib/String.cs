using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    // Always null terminated
    public sealed unsafe class String : IEnumerable<char>
    {
        public bool Equals(string other) => this == other;

        public override bool Equals(object obj) => obj is string other && Equals(other);

        public override int GetHashCode()
        {
            var i = 0;

            foreach (char c in this)
            {
                i ^= c;
            }

            return i;
        }

        public const string Empty = "";

#pragma warning disable 649
        private int _stringLength;
#pragma warning restore 649
        private char _firstChar;

        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public int Length => _stringLength;

        public ref readonly char GetPinnableReference() => ref _firstChar;

        [IndexerName("Chars")]
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

        // Do not use strcmp - it will not handle the {legal} null characters in the string properly
        [DllImport("libc")]
        private static extern int memcmp(char* left, char* right, IntPtr count);
        IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();
        public CharEnumerator GetEnumerator() => new CharEnumerator(this, 0u, (uint)_stringLength);
        public struct CharEnumerator : IEnumerator<char>, IEnumerable<char>
        {
            private readonly string _str;
            private int _index;
            private readonly int _length;

            internal CharEnumerator(string str, uint index, uint length) // for overload resolution. Skip checks for perf
            {
                _str = str;
                _index = (int)index - 1;
                _length = (int)length;
                Current = str[_index];
            }

            public CharEnumerator(string str, int index = 0, int length = -1)
            {
                if (length == -1) length = str.Length;
                if ((uint)length > str.Length || index > str.Length || index + length > str.Length)
                    ThrowHelper.ThrowIndexOutOfRangeException("Index was outside the bounds of the array.");

                _str = str;
                _index = index - 1;
                _length = length;
                Current = str[_index];
            }
            public bool MoveNext()
            {
                if (_index++ >= _length) return false;

                Current = _str[_index];
                return true;
            }

            public char Current { get; private set; }
            public void Reset()
            {
                _index = 0;
            }

            object IEnumerator.Current => Current;

            IEnumerator<char> IEnumerable<char>.GetEnumerator() => GetEnumerator();

            public CharEnumerator GetEnumerator() => this;
        }

    }
}