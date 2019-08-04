using System;
using System.Runtime.CompilerServices;
using NetInterface;
using NetRt.Assemblies;

namespace NetRt.TypeLoad.TypeSystem
{
    public sealed class PointerType : DerivativeType
    {
        public PointerType(TypeDefinition underlyingType) => UnderlyingType = underlyingType;

        public override int Size => Unsafe.SizeOf<object>();
        public override Field[] Fields => Array.Empty<Field>();
        public override MethodDef[] Methods => Array.Empty<MethodDef>();
        public override TypeDefinition UnderlyingType { get; }
        public override bool IsObject => false;
    }
}