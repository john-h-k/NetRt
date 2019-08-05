using System;
using NetInterface;
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
        public override byte* JitMethod(MethodInformation method)
        {
            _ilMemory = method.Il;
            _instructionReader = new InstructionReader(_ilMemory);
            CreateBasicBlocks();
            throw new NotImplementedException();
        }
    }
}
