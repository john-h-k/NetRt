using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using Common;

namespace NetJit.Representations
{
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public readonly struct Instruction
    {
        public Instruction(OpCode opCode, ReadOnlyMemory<byte> operand, int fullSize, int position)
        {
            OpCode = opCode;
            Operand = operand;
            FullSize = fullSize;
            Position = position;

            Debug.Assert(OpCode.OperandSize == Operand.Length);
        }

        public OpCode OpCode { get; }
        public ReadOnlyMemory<byte> Operand { get; }
        public int OperandSize => OpCode.OperandSize;
        public int FullSize { get; }
        public int Position { get; }

        private static readonly char[] LabelTemplate = "IL_00000000".ToCharArray();
        private const int LabelTemplateLeadingChars = 3;
        private const int LabelTemplateNumChars = 8;

        public static int TryFormatLabelSize(int index)
        {
            if (index < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));

            if (index <= 0xFFFF) return LabelTemplateLeadingChars + 4;
            if (index <= 0xFFFFF) return LabelTemplateLeadingChars + 5;
            if (index <= 0xFFFFFF) return LabelTemplateLeadingChars + 6;
            if (index <= 0xFFFFFFF) return LabelTemplateLeadingChars + 7;
            return LabelTemplateLeadingChars + 8;

        }

        public static bool TryFormatLabel(int index, Span<char> buffer, out int charsWritten)
        {
            if (index < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(index));

            Span<char> labelTemplate = LabelTemplate;

            if (!index.TryFormat(labelTemplate.Slice(LabelTemplateLeadingChars, LabelTemplateNumChars),
                out int newCharsWritten, "X4")) ThrowHelper.ThrowInvalidOperationException("Method too large");

            int labelLength = newCharsWritten + LabelTemplateLeadingChars;
            if (!labelTemplate.Slice(0, labelLength).TryCopyTo(buffer))
            {
                charsWritten = 0;
                return false;
            }

            charsWritten = labelLength;

            return true;
        }

        public int ReadBranchTarget()
        {
            Debug.Assert(OpCode.IsBranch);

            if (OpCode.OperandSize == 1)
            {
                return MemoryMarshal.Read<sbyte>(Operand.Span);
            }

            if (OpCode.OperandSize == 4)
            {
                return MemoryMarshal.Read<int>(Operand.Span);
            }

            ThrowHelper.ThrowInvalidOperationException("OpCode is not a branch");
            return default;
        }

        public T ReadOperandAs<T>() where T : unmanaged
        {
            return MemoryMarshal.Read<T>(Operand.Span);
        }

        private double GetRealOperandIfExists()
        {
            if (!OpCode.OperandParams.IsReal()) ThrowHelper.ThrowInvalidOperationException("Operand doesn't have a real operand");

            Debug.Assert(OpCode.OperandSize == 4 || OpCode.OperandSize == 8);

            return OpCode.OperandSize == 4 ? ReadOperandAs<float>() : ReadOperandAs<double>();

        }

        private long GetIntegralOperandIfExists()
        {
            if (OpCode.OperandParams.IsReal()) ThrowHelper.ThrowInvalidOperationException("Operand doesn't have an integer operand");

            long operand = OpCode.OperandSize switch
            {
                1 => ReadOperandAs<sbyte>(),

                2 => ReadOperandAs<short>(),

                4 => ReadOperandAs<int>(),

                8 => ReadOperandAs<long>(),

                _ => 0
            };

            return operand;
        }

        private string GetOperandAsString()
        {
            if (OpCode.OperandSize == 0) return string.Empty;
            if (OpCode.OperandParams.IsReal()) return GetRealOperandIfExists().ToString();

            return GetIntegralOperandIfExists().ToString();
        }

        public override string ToString()
        {
            string opCode = OpCode.ToString();

            string operand = GetOperandAsString();

            if (operand is null) ThrowHelper.ThrowInvalidOperationException("Invalid operand");

            return $"{opCode} {operand}";
        }

        public int TryFormatSize()
        {
            int opCodeLen = OpCode.TryFormatSize() + 1; // space
            if (OpCode.IsBranch)
            {
                return opCodeLen + TryFormatLabelSize((int)GetIntegralOperandIfExists() + Position + FullSize);
            }

            if (OpCode.OperandSize == 0)
            {
                return opCodeLen;
            }

            return opCodeLen + GetOperandAsString().Length; // TODO make better
        }

        public bool TryFormat(Span<char> buffer, out int charsWritten, int labelOffset = 0)
        {
            if (!OpCode.TryFormat(buffer, out charsWritten)) return false;

            if (OpCode.OperandSize == 0)
            {
                return true;
            }

            buffer = buffer.Slice(charsWritten);

            if (buffer.IsEmpty) return false;
            buffer[0] = ' ';
            buffer = buffer.Slice(1);
            charsWritten++;

            int newCharsWritten;
            if (OpCode.OperandParams.IsReal())
            {
                double operand = GetRealOperandIfExists();

                if (!operand.TryFormat(buffer, out newCharsWritten))
                {
                    charsWritten += newCharsWritten;
                    return false;
                }
            }
            else
            {
                long operand = GetIntegralOperandIfExists();

                if (OpCode.HasTarget)
                {
                    TryFormatLabel((int)operand + Position + FullSize + labelOffset, buffer, out newCharsWritten);
                }   
                else if (!operand.TryFormat(buffer, out newCharsWritten))
                {
                    charsWritten += newCharsWritten;
                    return false;
                }
            }

            charsWritten += newCharsWritten;
            return true;
        }

        public static bool operator ==(Instruction left, Instruction right) => left.OpCode == right.OpCode && left.Operand.Span.SequenceEqual(right.Operand.Span);

        public static bool operator !=(Instruction left, Instruction right) => !(left == right);
    }
}