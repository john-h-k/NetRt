using System;
using System.Buffers;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
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
            _ownedIl = il;
            MethodDataSection = methodDataSection;
        }


        public MethodDef RawMethodDef { get; }
        public uint Rva => RawMethodDef.Rva;
        public MethodAttributes Flags => RawMethodDef.Flags;
        public MethodImplOptions ImplFlags => RawMethodDef.ImplFlags;
        public ushort ParamList => RawMethodDef.ParamList;
        public uint Signature => RawMethodDef.Signature;
        public string Name => RawMethodDef.Name;
        public bool HasBody => Rva != 0;
        public MethodHeader Header { get; }
        public Memory<byte> Il => _ownedIl.Memory.Slice(0, checked((int)Header.CodeSize));
        private readonly IMemoryOwner<byte> _ownedIl;

        public MethodDataSection[] MethodDataSection { get; set; }

        public bool HasLocals => Header.LocalVarSigToken != 0;

        public void Dispose()
        {
            _ownedIl?.Dispose();
        }
    }
}