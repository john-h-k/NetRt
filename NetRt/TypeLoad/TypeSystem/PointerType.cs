using System;
using System.Runtime.CompilerServices;
using NetRt.Assemblies;
using NetRt.Metadata.TableElements;

namespace NetRt.TypeLoad.TypeSystem
{
    public sealed class PointerType : DerivativeType
    {
        public PointerType(TypeInformation underlyingType, uint token) : base(token) => UnderlyingType = underlyingType;

        public override int Size => Unsafe.SizeOf<object>();
        public override Field[] Fields => Array.Empty<Field>();
        public override MethodDef[] Methods => Array.Empty<MethodDef>();
        public override TypeInformation UnderlyingType { get; }
        public override bool IsObject => false;
    }
}