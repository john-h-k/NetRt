using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Common;
using ThrowHelper = NetRt.Common.ThrowHelper;

namespace NetRt.Assemblies.Heaps
{
    public class UserStringHeap : Heap
    {
        public UserStringHeap(Memory<byte> data) : base(data)
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

            Span<byte> span = Data.Span.Slice((int) index);

            int len = (int)Utils.ReadVarLenUInt32(ref span);
            len &= ~1;

            string s = MemoryMarshal.Cast<byte, char>(span.Slice(0, len)).ToString();
            _cache[index] = s;
            return s;
        }
    }
}