using System;
using Common;
using NetJit.Representations;
using OpCode = NetJit.Representations.OpCode;

// ReSharper disable InconsistentNaming

namespace NetJit
{
    public struct InstructionReader
    {
        public InstructionReader(Memory<byte> il)
        {
            Il = il;
            __backing_field__position = 0;
        }

        public Memory<byte> Il { get; }
        private Memory<byte> Current => Il.Slice(Position);

        private int __backing_field__position;
        public int Position
        {
            get => __backing_field__position;
            set
            {
                if (value < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));

                __backing_field__position = value;
            }
        }

        public void Advance(int count)
        {
            if (count < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(count));

            Position += count;
        }

        public Instruction ReadInstruction()
        {
            OpCode opCode = OpCode.ReadOpCode(ref Current.Span[Position]);
            Advance(opCode.Size);
            Memory<byte> operand = Current.Slice(0, opCode.OperandSize);
            Advance(opCode.OperandSize);

            return new Instruction(opCode, operand);
        }
    }
}