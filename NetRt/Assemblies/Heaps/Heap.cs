using System;

namespace NetRt.Assemblies.Heaps
{
    public abstract class Heap
    {
        protected Heap(Memory<byte> data) => Data = data;

        public Memory<byte> Data { get; }

        public int IndexSize { get; set; }
    }
}