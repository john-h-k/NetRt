using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetRt.Assemblies
{
    using Rva = UInt32;
    public readonly struct MethodDef
    {
        public MethodDef(uint rva, MethodImplOptions implFlags, MethodAttributes flags, string name, uint signature, ushort paramList)
        {
            Rva = rva;
            ImplFlags = implFlags;
            Flags = flags;
            Name = name;
            Signature = signature;
            ParamList = paramList;
        }

        public Rva Rva { get; }
        public MethodImplOptions ImplFlags { get; }
        public MethodAttributes Flags { get; }
        public string Name { get; }
        public uint Signature { get; }
        public ushort ParamList { get; }
    }
}