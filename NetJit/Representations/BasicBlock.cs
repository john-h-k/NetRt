// ReSharper disable InconsistentNaming

using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

#nullable enable

namespace NetJit.Representations
{
    public class BasicBlock
    {
        public BasicBlockFlags Flags { get; set; }
        public Memory<byte> Il { get; }
        public int Offset { get; }
        public int Length { get; }

        private BasicBlock? __backing_field__previous;
        private BasicBlock? __backing_field__next;

        public BasicBlock? Previous
        {
            get => __backing_field__previous;
            set
            {
                __backing_field__previous = value;
                if (value is object) value.__backing_field__next = this;
            }
        }

        public BasicBlock? Next
        {
            get => __backing_field__next;
            set
            {
                __backing_field__next = value;
                if (value is object) value.__backing_field__previous = this;
            }
        }

        public BasicBlock(BasicBlock? previous, BasicBlock? next, Memory<byte> method, int offset, int length)
        {
            Previous = previous;
            Next = next;
            Offset = offset;
            Length = length;
            Il = method.Slice(offset, length);
        }
    }

    public enum BasicBlockFlags : ulong
    {

    }
}