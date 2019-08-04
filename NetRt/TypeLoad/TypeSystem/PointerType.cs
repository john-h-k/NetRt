using System;
using NetInterface;

namespace NetRt.TypeLoad.TypeSystem
{
    public sealed class PointerType : DerivativeType
    {
        public override Field[] Fields => Array.Empty<Field>();
        public override MethodDef[] Methods => Array.Empty<MethodDef>();
        public override TypeDefinition UnderlyingType { get; }
        public override bool IsObject => false;
    }
}