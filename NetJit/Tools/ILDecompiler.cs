using System;
using System.Collections.Generic;
using Common;
using NetJit.Representations;
using NetRt.Metadata;

#nullable enable

namespace NetJit.Tools
{
    public readonly struct IlDecompiler
    {
        public ReadOnlyMemory<Instruction> Instructions { get; }

        public int Offset { get; }
        public int TabsPerLine { get; }

        public IlDecompiler(MethodInformation methodInformation) : this(methodInformation.Il)
        {
        }

        public IlDecompiler(ReadOnlyMemory<byte> il, int offset = 0, int tabsPerLine = 0) : this(new InstructionReader(il).ReadAllInstructionsToArray(), offset, tabsPerLine)
        {
        }

        public IlDecompiler(ReadOnlyMemory<Instruction> instructions, int offset = 0, int tabsPerLine = 0)
        {
            Instructions = instructions;
            Offset = offset;
            TabsPerLine = tabsPerLine;
        }

        public IlDecompiler Slice(int offset)
        {
            return new IlDecompiler(Instructions, offset);
        }

        public override string ToString()
        {
            return string.Create(TryDumpIlSize(), this, (span, comp) => comp.TryDumpIl(span, out _));
        }

        private static void Tab(ref Span<char> buffer, int tabsPerLine, ref int charsWritten)
        {
            for (var j = 0; j < tabsPerLine; j++)
            {
                buffer[j] = '\t';
            }

            charsWritten += tabsPerLine;
            buffer = buffer.Slice(tabsPerLine);
        }

        public bool TryDumpIl(Span<char> buffer, out int charsWritten, bool encloseInBracketsAndTab = false)
        {
            // Format each instruction as
            // IL_{byteAddressOfInstruction} {instructionAlias} {operands}
            // and then a newline

            charsWritten = 0;


            if (encloseInBracketsAndTab)
            {
                if (buffer.IsEmpty) return false;
                Tab(ref buffer, TabsPerLine, ref charsWritten);
                buffer[0] = '{';
                buffer = buffer.Slice(1);
                charsWritten++;
            }

            int tabsPerLine = encloseInBracketsAndTab ? TabsPerLine + 1 : TabsPerLine;

            for (var i = 0; i < Instructions.Length; i++)
            {
                if (buffer.Length < tabsPerLine) return false;
                Tab(ref buffer, tabsPerLine, ref charsWritten);

                Instruction instr = Instructions.Span[i];

                if (!Instruction.TryFormatLabel(instr.Position + Offset, buffer, out int newCharsWritten))
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

            if (encloseInBracketsAndTab)
            {
                if (buffer.IsEmpty) return false;
                Tab(ref buffer, tabsPerLine, ref charsWritten);
                buffer[0] = '}';
                buffer = buffer.Slice(1);
                charsWritten++;
            }

            return true;
        }

        public int TryDumpIlSize()
        {
            var size = 0;

            for (var i = 0; i < Instructions.Length; i++)
            {
                Instruction instr = Instructions.Span[i];

                size += Instruction.TryFormatLabelSize(i);

                size += 2;

                size += instr.TryFormatSize();

                size++;
            }

            return size;
        }
    }
}