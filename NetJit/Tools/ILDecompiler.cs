using System;
using Common;
using NetJit.Representations;
using NetRt.Metadata;

#nullable enable

namespace NetJit.Tools
{
    public readonly struct IlDecompiler
    {
        public MethodInformation? _Method;
        public Memory<byte> Il { get; }
        public int Offset { get; }

        public IlDecompiler(MethodInformation methodInformation)
        {
            Il = methodInformation.Il;
            Offset = 0;
        }

        public IlDecompiler(Memory<byte> il, int offset)
        {
            Il = il;
            Offset = offset;
        }

        public IlDecompiler Slice(int offset)
        {
            return new IlDecompiler(Il, Offset + offset);
        }

        public override string ToString()
        {
            return string.Create(TryDumpIlSize(), this, (span, comp) => comp.TryDumpIl(span, out _));
        }

        public bool TryDumpIl(Span<char> buffer, out int charsWritten)
        {
            // Format each instruction as
            // IL_{byteAddressOfInstruction} {instructionAlias} {operands}
            // and then a newline

            var reader = new InstructionReader(Il);
            Instruction instr;

            charsWritten = 0;
            for (var i = 0; i < Il.Length; i += instr.FullSize)
            {
                instr = reader.ReadInstruction();

                if (!Instruction.TryFormatLabel(i + Offset, buffer, out int newCharsWritten))
                {
                    charsWritten += newCharsWritten;
                    return false;
                }

                charsWritten += newCharsWritten;
                buffer = buffer.Slice(newCharsWritten);

                if (buffer.Length < 2) return false;
                buffer[0] = ':';
                buffer[1] = ' ';
                charsWritten += 2;
                buffer = buffer.Slice(2);

                if (!instr.TryFormat(buffer, out newCharsWritten, Offset))
                {
                    charsWritten += newCharsWritten;
                    return false;
                }

                charsWritten += newCharsWritten;
                buffer = buffer.Slice(newCharsWritten);

                if (buffer.IsEmpty) return false;
                buffer[0] = '\n';
                charsWritten++;
                buffer = buffer.Slice(1);
            }

            return true;
        }

        public int TryDumpIlSize()
        {
            var size = 0;

            var reader = new InstructionReader(Il);
            Instruction instr;

            for (var i = 0; i < Il.Length; i += instr.FullSize)
            {
                instr = reader.ReadInstruction();

                size += Instruction.TryFormatLabelSize(i);

                size += 2;

                size += instr.TryFormatSize();

                size++;
            }

            return size;
        }
    }
}