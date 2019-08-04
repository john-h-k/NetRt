using System;
using NetRt.Common;

namespace NetRt.Assemblies.Heaps
{
    public class BlobHeap : Heap
    {
        public BlobHeap(Memory<byte> data) : base(data)
        {
        }

        public ReadOnlySpan<byte> Read(uint index)
        {
            return GetWriteableView(index);
        }

        public Span<byte> GetWriteableView(uint index)
        {
            if ((int)index < 0 || index >= Data.Length)
                ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));
            if (index == 0)
                return Array.Empty<byte>();

            Span<byte> span = Data.Span.Slice((int)index);
            int len = Utils.ReadVarLenUInt32(ref span);

            if (len > Data.Length - index)
                ThrowHelper.ThrowInvalidOperationException(NetRtResources.GetResource("DataLargerThanHeap"));

            return span.Slice(0, len);

        }
    }
}