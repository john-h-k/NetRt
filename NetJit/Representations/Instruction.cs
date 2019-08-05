using System;

namespace NetJit.Representations
{
    public readonly struct Instruction
    {
        public Instruction(OpCode opCode, Memory<byte> operand)
        {
            OpCode = opCode;
            Operand = operand;
        }

        public OpCode OpCode { get; }
        public Memory<byte> Operand { get; }
    }
}