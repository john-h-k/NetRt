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

        private Memory<byte> _il;
        public override byte* JitMethod(MethodInformation method)
        {
            _il = method.Il;
        }
    }
}
