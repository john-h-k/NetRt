using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Common;
using NetJit.Representations;
using OpCode = NetJit.Representations.OpCode;

// ReSharper disable InconsistentNaming

namespace NetJit
{
    public struct InstructionReader : IEnumerable<Instruction>, IEnumerator<Instruction>
    {
        private readonly List<Instruction> _instructions;

        public InstructionReader(ReadOnlyMemory<byte> il)
        {
            Il = il;
            __backing_field__position = 0;
            _instructions = new List<Instruction>(il.Length / 2); // Guess each instruction is 2 bytes
                                                                 // (if this changes, keep it a pow of 2 for perf. this must be a fast type!!
            Current = default;
        }

        public ReadOnlyMemory<byte> Il { get; }

        object IEnumerator.Current => Current;

        private ReadOnlyMemory<byte> CurrentIl => Il.Slice(Position);

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

        public bool TryReadInstruction(out Instruction instr)
        {
            int opPosition = Position;
            if (!OpCode.TryReadOpCode(CurrentIl.Span, out OpCode opCode))
            {
                instr = default;
                return false;
            }

            Advance(opCode.Size);

            if (CurrentIl.Length < opCode.OperandSize)
            {
                instr = default;
                return false;
            }

            ReadOnlyMemory<byte> operand = CurrentIl.Slice(0, opCode.OperandSize);
            Advance(opCode.OperandSize);

            instr = new Instruction(opCode, operand, opCode.Size + opCode.OperandSize, opPosition);

            Current = instr;

            return true;
        }

        public bool MoveNext()
        {
            return TryReadInstruction(out _);
        }

        public void Reset()
        {
            Position = 0;
        }

        public Instruction Current { get; private set; }

        public Instruction[] ReadAllInstructionsToArray()
        {
            foreach (Instruction instr in this)
            {
                _instructions.Add(instr);
            }

            return _instructions.ToArray();
        }

        void IDisposable.Dispose()
        {

        }

        public InstructionReader GetEnumerator() => this;
        IEnumerator<Instruction> IEnumerable<Instruction>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


}