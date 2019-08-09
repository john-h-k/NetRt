using System;
using System.Buffers;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using NetRt.Assemblies;

namespace NetRt.Metadata
{
    public class MethodInformation : IDisposable
    {

        public MethodInformation(MethodDef rawMethodDef, MethodHeader methodHeader, IMemoryOwner<byte> il, MethodDataSection[] methodDataSections)
        {
            RawMethodDef = rawMethodDef;
            Header = methodHeader;
            _ownedIl = il;
            MethodDataSections = methodDataSections;
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
        public MethodDataSection[] MethodDataSections { get; set; }
        public bool HasLocals => Header.LocalVarSigToken != 0;

        public override string ToString()
        {
            return $"Method {Name} [{(HasBody ? "HasBody" : "NoImpl")}]:\n" +
                   $"{(HasBody ? $"Method begins at RVA {Rva}, with CodeSize {Header.CodeSize}" : string.Empty)}\n" +
                   $"MethodAttributes: {Flags}\n" +
                   $"MethodImplOptions: {ImplFlags}\n" +
                   $"HasLocals: {HasLocals}\n" +
                   $"HasEh: {MethodDataSections.Length > 0 && MethodDataSections[0].ExceptionHandlingClauses.Length != 0}\n" +
                   $"{(MethodDataSections.Length > 0 ? $"MethodDataSections: {string.Join(separator: ' ', MethodDataSections.Select(sect => sect.ToString()))}" : string.Empty)}";
    }

        public void Dispose()
        {
            _ownedIl?.Dispose();
        }
    }
}