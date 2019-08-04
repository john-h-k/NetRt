using System;
using NetInterface;
using NetRt.Assemblies;
using NetRt.Interfaces;

namespace NetJit
{
    public sealed unsafe partial class Compiler : Jit
    {
        private Memory<byte> _methodBytes;
        public override byte* JitMethod(MethodDef method, ref byte il)
        {
            _methodBytes = ScanIlForEnd(ref il);
        }
    }
}
