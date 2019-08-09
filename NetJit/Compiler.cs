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
using NetRt.TypeLoad.TypeSystem;

namespace NetJit
{
    public sealed unsafe partial class Compiler : Jit
    {
        static Compiler()
        {
            Instance = new Compiler();
        }

        private LocalVar[] _locals;
        private LocalVar[] _args;
        private Memory<byte> _ilMemory;
        private Span<byte> Il => _ilMemory.Span;
        private MethodInformation _methodInformation;
        private IlDecompiler _decompiler;
        private int _bbCount;

        public override void Initialize(MethodInformation method)
        {
            _decompiler = new IlDecompiler(method);
            _methodInformation = method;
            _ilMemory = method.Il;
            _instructionReader = new InstructionReader(_ilMemory);
        }

        public override byte* JitMethod()
        {
            CreateBasicBlocks();
            CreateExpressionTrees();
            return null; // TODO
        }

        public override string DumpIl()
        {
            return _decompiler.ToString();
        }

        public override string CreateJitDump()
        {
            const string divider = "********************************************************************************";

            BasicBlock b = _first;
            var builder = new StringBuilder();

            builder.AppendLine("Method IL: ").Append(new IlDecompiler(_ilMemory, 0)).Append('\n');

            builder.AppendLine(divider);
            builder.AppendLine("Stage 0: BasicBlock importation");
            builder.AppendLine(divider);

            builder.Append("Parsed ").Append(_bbCount).AppendLine(" BasicBlocks");
            builder.AppendLine("BasicBlocks: \n");

            var i = 0;
            do
            {
                builder.Append("BasicBlock ").Append(i++).Append(':').AppendLine();
                var decompiler = new IlDecompiler(b.Il, b.Offset);
                builder.AppendLine(decompiler.ToString());

                builder.Append('\n');

                b = b.Next;
            } while (b is object);

            builder.AppendLine(divider);
            builder.AppendLine("END OF DUMP");
            builder.AppendLine(divider);
            return builder.ToString();
        }

        public override bool TryDumpIl(Span<char> buffer, out int charsWritten)
        {
            return _decompiler.TryDumpIl(buffer, out charsWritten);
        }
    }

    // .locals and args
    public readonly struct LocalVar
    {
        public LocalVar(TypeInformation type, bool pinned)
        {
            Type = type;
            Pinned = pinned;
        }

        public TypeInformation Type { get; }
        public bool Pinned { get; }
    }
}
