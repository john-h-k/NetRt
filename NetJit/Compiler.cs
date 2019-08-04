using System;
using NetInterface;
using NetRt.Assemblies;
using NetRt.Interfaces;

namespace NetJit
{
    public sealed class Compiler : Jit
    {
        public override unsafe byte* JitMethod(MethodDef method, byte* il)
        {
            throw new NotImplementedException();
        }
    }
}
