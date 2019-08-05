using System;
using System.Buffers;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NetRt.Assemblies;

namespace NetRt.Metadata
{
    public class MethodInformation : IDisposable
    {

        public MethodInformation(MethodDef rawMethodDef, MethodHeader methodHeader, IMemoryOwner<byte> il, MethodDataSection[] methodDataSection)
        {
            RawMethodDef = rawMethodDef;
            Header = methodHeader;
            OwnedIl = il;
            MethodDataSection = methodDataSection;
        }


        public Assemblies.MethodDef RawMethodDef { get; }
        public uint Rva => RawMethodDef.Rva;
        public MethodAttributes Flags => RawMethodDef.Flags;
        public MethodImplOptions ImplFlags => RawMethodDef.ImplFlags;
        public ushort ParamList => RawMethodDef.ParamList;
        public uint Signature => RawMethodDef.Signature;
        public string Name => RawMethodDef.Name;
        public bool HasBody => Rva != 0;
        public MethodHeader Header { get; }
        public Memory<byte> Il => OwnedIl.Memory;
        private readonly IMemoryOwner<byte> OwnedIl;

        public MethodDataSection[] MethodDataSection { get; set; }

        public bool HasLocals => Header.LocalVarSigToken != 0;

        public void Dispose()
        {
            Il?.Dispose();
        }
    }
}