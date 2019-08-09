using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
            int opPosition = Position;
            OpCode opCode = OpCode.ReadOpCode(ref Il.Span[Position]);
            Advance(opCode.Size);
            Memory<byte> operand = Current.Slice(0, opCode.OperandSize);
            Advance(opCode.OperandSize);

            return new Instruction(opCode, operand, opCode.Size + opCode.OperandSize, opPosition);
        }

        public void FollowBranch(Instruction branch)
        {
            Debug.Assert(branch.OpCode.IsBranch);
            Debug.Assert(branch.OpCode.OperandSize == 1 || branch.OpCode.OperandSize == 4);
            Advance((branch.OpCode.OperandSize == 1 ? MemoryMarshal.Read<byte>(branch.Operand.Span) : MemoryMarshal.Read<int>(branch.Operand.Span)) + 1);
        }
    }
}