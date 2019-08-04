using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public sealed class String : Object
    {
        private int _stringLength;
        private char _firstChar;

        // ReSharper disable once ConvertToAutoProperty
        public int Length => _stringLength;

        public ref readonly char GetPinnableReference() => ref _firstChar;

        public char this[int index]
        {
            [Intrinsic]
            get
            {
                if ((uint)index > (uint)_stringLength)
                {
                    throw null; // TODO
                }

                return Unsafe.Add(ref _firstChar, index);
            }
        }
    }
}