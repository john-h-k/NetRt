using System;
using System.Buffers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Common;
using NetInterface;
using NetJit.Representations;
using NetJit.Tools;
using NetRt.Assemblies;
using NetRt.Interfaces;
using NetRt.Metadata;

namespace NetJit
{
    public sealed unsafe partial class Compiler : Jit
    {
        static Compiler()
        {
            Instance = new Compiler();
        }

        private Memory<byte> _ilMemory;
        private Span<byte> Il => _ilMemory.Span;
        private MethodInformation _methodInformation;
        private ILDecompiler _decompiler;

        public override void Initialize(MethodInformation method)
        {
            _decompiler = new ILDecompiler(method);
            _methodInformation = method;
            _ilMemory = method.Il;
            _instructionReader = new InstructionReader(_ilMemory);
        }

        public override byte* JitMethod()
        {
            CreateBasicBlocks();
            return null; // TODO
        }

        public override string DumpIl()
        {
            return _decompiler.ToString();
        }

        public override string CreateJitDump()
        {
            BasicBlock b = _first;
            var builder = new StringBuilder();

            int i = 0;
            do
            {
                builder.Append("BasicBlock ").Append(i++).Append(':').AppendLine();
                _decompiler = new ILDecompiler(b.Il, b.Offset);
                builder.AppendLine(_decompiler.ToString());

                builder.Append('\n');

                b = b.Next;
            } while (b is object);

            return builder.ToString();
        }

        public override bool TryDumpIl(Span<char> buffer, out int charsWritten)
        {
            return _decompiler.TryDumpIl(buffer, out charsWritten);
        }
    }
}
