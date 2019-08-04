using System;
using System.Collections.Generic;
using System.Text;
using NetRt.Common;

namespace NetRt.Assemblies.Heaps
{
    // UTF8 strings :(
    public class StringHeap : Heap
    {
        public StringHeap(Memory<byte> data) : base(data)
        {
        }

        private readonly Dictionary<uint, string> _cache = new Dictionary<uint, string>();

        public string GetString(uint index)
        {
            if (index == 0) return string.Empty;
            if (index >= Data.Length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));

            if (_cache.TryGetValue(index, out string str))
            {
                return str;
            }

            int nullChar = Data.Span.Slice((int)index).IndexOf((byte)0);
            str = Encoding.UTF8.GetString(Data.Span.Slice((int)index, nullChar));
            _cache[index] = str;
            return str;
        }
    }
}