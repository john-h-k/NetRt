using System;
using System.Runtime.InteropServices;
using NetRt.Common;

namespace NetRt.Assemblies.Heaps
{
    public class GuidHeap : Heap
    {
        public GuidHeap(Memory<byte> data) : base(data)
        {
        }

        public Guid Read(uint index)
        {
            if (index == 0)
                return Guid.Empty;

            if (index + 16 >= Data.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));

            Span<Guid> guids = MemoryMarshal.Cast<byte, Guid>(Data.Span);
            return guids[(int)(index - 1)];
        }
    }
}