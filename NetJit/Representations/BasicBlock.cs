// ReSharper disable InconsistentNaming

using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

#nullable enable

namespace NetJit.Representations
{
    public class BasicBlock
    {
        private BasicBlock? __backing_field__previous;
        private BasicBlock? __backing_field__next;

        public BasicBlock(BasicBlock? previous, BasicBlock? next, Memory<byte> il)
        {
            Previous = previous;
            Next = next;
            Il = il;
        }

        public BasicBlock? Previous
        {
            get => __backing_field__previous;
            set
            {
                __backing_field__previous = value;
                if (value is object) value.Next = this;
            }
        }

        public BasicBlock? Next
        {
            get => __backing_field__next;
            set
            {
                __backing_field__next = value;
                if (value is object) value.Previous = this;
            }
        }

        public BasicBlockFlags Flags { get; set; }
        public Memory<byte> Il { get; set; }
        
    }

    public enum BasicBlockFlags : ulong
    {

    }
}