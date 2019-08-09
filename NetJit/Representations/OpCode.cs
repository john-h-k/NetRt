using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.VisualBasic.CompilerServices;
using static NetJit.Representations.OpCodesDef;

namespace NetJit.Representations
{
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public readonly struct OpCode
    {
        public override string ToString() => Alias;

        public int TryFormatSize() => Alias.Length;

        public bool TryFormat(Span<char> buffer, out int charsWritten)
        {
            if (!Alias.AsSpan().TryCopyTo(buffer))
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = Alias.Length;
            return true;
        }

        public bool Equals(OpCode other)
        {
            return string.Equals(Alias, other.Alias) &&
                   PopBehaviour == other.PopBehaviour && PushBehaviour == other.PushBehaviour &&
                   OperandParams == other.OperandParams && OpCodeKind == other.OpCodeKind && Size == other.Size &&
                   FirstByte == other.FirstByte && SecondByte == other.SecondByte &&
                   ControlFlowKind == other.ControlFlowKind;
        }

        public override bool Equals(object obj)
        {
            return obj is OpCode other && Equals(other);
        }

        public override int GetHashCode()
        {
            return FirstByte | (SecondByte << 8);
        }

        private OpCode(string alias, PopBehaviour popBehaviour, PushBehaviour pushBehaviour, OperandParams operandParams, OpCodeKind opCodeKind, int size, byte firstByte, byte secondByte, ControlFlowKind controlFlowKind)
        {
            Alias = alias;
            PopBehaviour = popBehaviour;
            PushBehaviour = pushBehaviour;
            OperandParams = operandParams;
            OpCodeKind = opCodeKind;
            Size = size;
            FirstByte = firstByte;
            SecondByte = secondByte;
            ControlFlowKind = controlFlowKind;
        }


        private static ushort BytesToUInt16(byte a, byte b) => (ushort)(a | (b << 8));
        public static OpCode Create(string alias, PopBehaviour popBehaviour, PushBehaviour pushBehaviour, OperandParams operandParams, OpCodeKind opCodeKind, int size, byte firstByte, byte secondByte, ControlFlowKind controlFlowKind)
        {
            var opCode = new OpCode
            (
                alias,
                popBehaviour,
                pushBehaviour,
                operandParams,
                opCodeKind,
                size,
                firstByte,
                secondByte,
                controlFlowKind
            );

            OpCodeMap[(firstByte, secondByte)] = opCode;

            return opCode;
        }

        public static bool operator ==(OpCode left, OpCode right) =>
            left.FirstByte == right.FirstByte && left.SecondByte == right.SecondByte;

        public static bool operator !=(OpCode left, OpCode right) => !(left == right);

        public static OpCode ReadOpCode(ref byte il)
        {
            byte first = il;
            byte second;
            // 2 byte encoding
            if (first == 0xFE)
            {
                second = Unsafe.Add(ref il, 1);
            }
            else
            {
                // single byte encoding, prefix with 0xFF to get the OpCode
                second = first;
                first = 0xFF;
            }

            return OpCodeMap[(first, second)];
        }

        // First byte being 0xFF represents only to use second byte
        public bool IsSingleByte => FirstByte == 0xFF;

        public string Alias { get; }
        public PopBehaviour PopBehaviour { get; }
        public PushBehaviour PushBehaviour { get; }
        public OperandParams OperandParams { get; }
        public int OperandSize => GetSizeForParamKind(OperandParams);
        public OpCodeKind OpCodeKind { get; }
        public int Size { get; }
        public byte FirstByte { get; }
        public byte SecondByte { get; }
        public ControlFlowKind ControlFlowKind { get; }

        public bool IsBranch => FirstByte == 0xFF && (SecondByte >= 0x2b && SecondByte <= 0x45);
        public bool IsUnconditionalBranch => this == Br || this == Br_S;
        public bool IsEndOfMethod => this == Ret || this == Jmp;
        public bool IsConditionalBranch => IsBranch && (this != Br && this != Br_S);
    }

    public static class OpCodeExtensions
    {
        public static bool IsInteger(this OperandParams operandParams)
        {
            return operandParams == OperandParams.InlineI
                   || operandParams == OperandParams.InlineI8
                   || operandParams == OperandParams.ShortInlineI;
        }

        public static bool IsReal(this OperandParams operandParams)
        {
            return operandParams == OperandParams.InlineR
                   || operandParams == OperandParams.ShortInlineR;
        }

        public static bool IsToken(this OperandParams operandParams)
        {
            switch (operandParams)
            {
                case OperandParams.InlineSig:
                case OperandParams.InlineMethod:
                case OperandParams.InlineType:
                case OperandParams.InlineString:
                case OperandParams.InlineField:
                case OperandParams.InlineTok:
                    return true;
                default:
                    return false;
            }
        }
    }
}